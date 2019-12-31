import ax from '@/utils/axios'

export const getUserList = params => ax.get('/user/getUserPageList', {
  params
}) // 列表
export const delUser = params => ax.post('/user/delUser', params) // 删除
export const editUser = params => ax.post('/user/editUser', params) // 编辑/创建
export const resetUserPwd = params => ax.post('/user/resetUserPwd', params)

export const getRoleList = () => ax.get('/role/getRoleList')
