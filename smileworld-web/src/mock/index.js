import app from './app'
import users from './users'
import test from './test'
import conference from './conference'

export default () => {

  const env = localStorage.getItem('env') || process.env.VUE_APP_ENV

  switch (env) {
    case 'test':
      console.log('Tset环境');
      break;
    case 'release':
      console.log('release环境');
      break;
    case 'production':
      console.log('production环境');
      break;
    default:
      console.log('development环境');
      break;
  }

  if (env === 'development') {
    // 登录，菜单，改密
    app()
    // 用户管理
    users()
    //媒体
    conference()
    // 测试
    //test()
  }
}
