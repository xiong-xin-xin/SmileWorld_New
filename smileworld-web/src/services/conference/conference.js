import ax from '@/utils/axios'
// 主子数据库列表
export const getNewsUserPageList = params => ax.get('/conference/getNewsUserPageList', {
  params
})
export const editNewsUser = params => ax.post('/conference/editNewsUser', params) // 编辑/创建
export const batchDelNewsUser = params => ax.post('/conference/batchDelNewsUser', params) // 批量删除
export const importDuplicate = params => ax.post('/conference/importDuplicate', params) // 导入子数据库

export const exportNewsUser = params => ax.get('/conference/exportNewsUser', {
  params
}) // 导出数据

export const getDuplicateList = () => ax.get('/conference/getDuplicateList') // 获取子数据库下拉框列表
//获取修改记录
export const getNewsUserLogList = params => ax.get('/conference/getNewsUserLogList', {
  params
})
//获取媒体列表下拉框
export const getMediaTypeList = () => ax.get('/conference/getMediaTypeList');



// 子数据库列表
export const getNewsConferenceUserPageList = params => ax.get('/conference/getNewsConferenceUserPageList', {
  params
})
export const editNewsConferenceUser = params => ax.post('/conference/EditNewsConferenceUser', params) // 编辑/创建
export const batchDelNewsConferenceUser = params => ax.post('/conference/BatchDelNewsConferenceUser', params) // 批量删除


export const exportNewsConferenceUser = params => ax.get('/conference/exportNewsConferenceUser', {
  params
}) // 导出数据


export const getMediaLevelList = () => ax.get('/conference/getMediaLevelList');
