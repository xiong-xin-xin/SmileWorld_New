import ax from '@/utils/axios'

export const getUserList = params => ax.get('/sysUser/getUserPageList', {
  params
}) // 列表
export const delUser = params => ax.post('/sysUser/delUser', params) // 删除
export const editUser = params => ax.post('/sysUser/editUser', params) // 编辑/创建
export const resetUserPwd = params => ax.post('/sysUser/resetUserPwd', params)

export const getRoleList = () => ax.get('/role/getRoleList')
