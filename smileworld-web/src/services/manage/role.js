import ax from '@/utils/axios'

export const getRolePageList = params => ax.get('/role/getRolePageList', {
  params
}) // 列表
export const delRole = params => ax.post('/role/delRole', params) // 删除
export const editRole = params => ax.post('/role/editRole', params) // 编辑/创建

export const getRoleAuthList = params => ax.get('/role/getRoleAuthList', {
  params
})
export const saveRoleAuth = params => ax.post('/role/saveRoleAuth', params) //保存权限
