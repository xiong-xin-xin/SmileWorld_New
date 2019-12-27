import ax from '@/utils/axios'


//销量上报表
export const getMobileSalePageList = params => ax.get('/mobileSale/getMobileSalePageList', {
  params
}) // 列表

//修改销量
export const editMobileSale = params => ax.post('/mobileSale/editMobileSale', params)

//导出
export const exportMobileSale = params => ax.get('/mobileSale/exportMobileSale', {
  params
})
//删除
export const delMobileSale = params => ax.post('/mobileSale/delMobileSale', params)

export const addMobileSale = params => ax.post('/mobileSale/addMobileSale', params)

export const alterMobileSale = params => ax.post('/mobileSale/alterMobileSale', params)


export const getMobileSale = params => ax.get('/mobileSale/getMobileSale', {
  params
})
