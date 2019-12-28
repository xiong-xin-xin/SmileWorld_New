import ax from '@/utils/axios'

export const getUserList = params => ax.get('/User/getUserPageList', {
  params
}) // 列表
export const delUser = params => ax.post('/User/delUser', params) // 删除
export const editUser = params => ax.post('/User/editUser', params) // 编辑/创建
export const resetUserPwd = params => ax.post('/User/resetUserPwd', params)

export const getRoleList = () => ax.get('/role/getRoleList')
