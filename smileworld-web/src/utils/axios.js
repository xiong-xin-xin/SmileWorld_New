import axios from 'axios'
import qs from 'qs'
import config from '@/config'
import store from '@/store'

const {
  env,
  baseURL,
  timeout
} = config
// 创建 axios 实例
const ax = axios.create({
  baseURL: localStorage.getItem('newBaseAPI') || baseURL, // 配置接口地址
  // 修改请求的数据再发送到服务器
  transformRequest: [
    (data, headers) => qs.stringify(data) // 序列化请求的数据
  ],
  timeout: timeout // 配置请求超时
})
// 请求拦截
ax.interceptors.request.use(config => {
  if (config.url.indexOf('export') > -1) {
    config['responseType'] = 'blob';
  }
  return config
}, error => {
  return Promise.reject(error)
})
// 添加响应拦截器
ax.interceptors.response.use(response => {
  const {
    code
  } = response.data
  // 用户 TOKEN 失效
  if (code === 3000) {
    store.commit('MENU_RESET') // 重置菜单
  }
  // 判断开发环境
  if (env === 'development' || env === 'test') {
    if (code === 0) {
      console.log(response.data) // 控制台输出响应数据
      return response.data
    }
    if (response.config.responseType === 'blob') {
      return response.data
    }
    if (response.status !== 200)
      store.commit('RES_ERROR', response) // 响应错误数据
    return response.data
  } else {
    return response.data
  }
}, error => {
  const {
    response,
    message
  } = error
  if (response) {
    if (response.status === 401) {
      store.commit('MENU_RESET') // 重置菜单
      window.location.reload()
    } else {
      if (env === 'development' || env === 'test') {
        store.commit('RES_ERROR', response) // 响应错误数据
      }
    }
  }
  console.log(message)
  return error
})

const userInfo = JSON.parse(sessionStorage.getItem('userInfo'))
// 配置默认参数
const setAuthToken = auth_token => {
  // 配置用户 AUTH_TOKEN
  ax.defaults.headers.common['Authorization'] = 'Bearer ' + auth_token.token;
}

// 刷新重新配置默认参数
if (userInfo) {
  setAuthToken(userInfo.auth_token) // 配置默认参数
}

//请求资源
export const get = (url, params) => ax.get(url, {
  params: params
})
//添加资源
export const post = (url, params) => ax.post(url, params)
//完整替换某个资源。
export const put = (url, params) => ax.put(url, params)
//部分替换某个资源
export const patch = (url, params) => ax.patch(url, params)
//删除
export const del = (url, params) => ax.delete(url, {
  params: params
})

export default ax
