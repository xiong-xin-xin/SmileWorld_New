import ax from '@/utils/axios'


//机型维护
export const getMobileTypePageList = params => ax.get('/mobileType/getMobileTypePageList', {
  params
}) // 列表
export const delMobileType = params => ax.post('/mobileType/delMobileType', params) // 删除
export const editMobileType = params => ax.post('/mobileType/editMobileType', params) // 编辑/创建
export const getMobileTypeList = params => ax.get('/mobileType/getMobileTypeList', {
  params
}) //获取该渠道下所有机型
export const editMobileTypeEnable = params => ax.get('/mobileType/editMobileTypeEnable', {
  params
}) // 修改状态


//渠道
export const getChannelPageList = params => ax.get('/mobileType/getChannelPageList', {
  params
}) // 列表
export const delChannel = params => ax.post('/mobileType/delChannel', params) // 删除
export const editChannel = params => ax.post('/mobileType/editChannel', params) // 编辑/创建
export const getChannelList = () => ax.get('/mobileType/getChannelList')


//品牌
export const getBrandPageList = params => ax.get('/mobileType/getBrandPageList', {
  params
}) // 列表
export const delBrand = params => ax.post('/mobileType/delBrand', params) // 删除
export const editBrand = params => ax.post('/mobileType/editBrand', params) // 编辑/创建
export const getBrandList = params => ax.post('/mobileType/getBrandList', params)
