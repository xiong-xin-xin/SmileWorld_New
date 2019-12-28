const version = '1.0.0' // 系统版本
const timeout = 10000 // 请求超时
const baseAPI = {
  development: 'http://development.xx.com/api',
  test: 'http://localhost:8085/admin',
  release: 'http://172.20.222.47:8888/api',
  production: 'admin'
}
const env = localStorage.getItem('env') || process.env.VUE_APP_ENV
const baseURL = localStorage.getItem('newBaseAPI') || baseAPI[env]

console.log('env：' + env)
if (env !== 'production') {
  console.log('VUE_APP_ENV', env)
  console.log('API_URL', baseURL)
}

// 系统参数配置
export default {
  version, // 系统版本
  timeout, // 请求超时
  baseAPI, // 所有环境接口对象
  env, // 当前环境
  baseURL // 当前环境接口地址
}
