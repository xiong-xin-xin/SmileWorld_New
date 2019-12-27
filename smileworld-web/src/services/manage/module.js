import ax from '@/utils/axios'

export const getModuleList = params => ax.get('/module/getModulePageList', {
  params
}) // 列表
export const delModule = params => ax.post('/module/delModule', params) // 删除
export const editModule = params => ax.post('/module/editModule', params) // 编辑/创建
