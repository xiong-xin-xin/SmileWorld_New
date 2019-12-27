import Vue from 'vue'
import App from '@/App.vue'
import router from '@/router'
import store from '@/store'
// iView 完整引入
import ViewUI from 'view-design';
// import style
import 'view-design/dist/styles/iview.css';
// 自定义组件引入
import iComp from '@/components'
// 工具函数引入
import Utils from '@/utils'
// 自定义组件引入
import vComp from '@/views/components'

import ZkTable from 'vue-table-with-tree-grid'
// 引入样式
import 'vue-easytable/libs/themes-base/index.css'
// 导入 table 和 分页组件
import {
  VTable,
  VPagination
} from 'vue-easytable'

// 注册到全局
Vue.component(VTable.name, VTable)
Vue.component(VPagination.name, VPagination)

// Mock 数据引入
import Mock from '@/mock'
// if (process.env.NODE_ENV === 'development') {
Mock()
// }

// 调用 `iView`
Vue.use(ViewUI)
Vue.use(ZkTable)
// 调用 `iComp`
Vue.use(iComp)
// 调用 `vComp`
Vue.use(vComp)

Object.defineProperty(Vue.prototype, '$Utils', {
  value: Utils
})

Vue.config.productionTip = false

new Vue({
  router,
  store,
  render: h => h(App),
}).$mount('#app')
