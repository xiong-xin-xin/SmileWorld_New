import ax from '@/utils/axios'

export const login = params => ax.get('/login/token', {
  params
}) // 用户登录
export const getUserMenuList = () => ax.get('/module/getUserModuleList') // 获取菜单
export const editPwd = params => ax.post('/sysUser/editPassword', params) // 修改密码
