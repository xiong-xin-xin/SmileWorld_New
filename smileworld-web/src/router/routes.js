// `routes` 配置
export default [{
    path: '/login',
    name: 'Login',
    component: () => import('@/layouts/Login')
  },
  {
    path: '*',
    name: 'NotFound',
    component: () => import('@/layouts/pages/NotFound')
  }
]
