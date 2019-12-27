import ax from '@/utils/axios'

export const getModuleButtonList = params => ax.get('/module/getModuleButtonList', {
  params
}) // 列表
export const delModuleButton = params => ax.post('/module/delModuleButton', params) // 删除
