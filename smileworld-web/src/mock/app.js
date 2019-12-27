import Mock from 'mockjs'
import qs from 'qs'

export default () => {
  // 用户登录
  Mock.mock(/\/login/, config => {
    const {
      username,
      password
    } = qs.parse(config.url.split('?')[1])
    if (username === 'admin' && password === '123456') {
      return {
        data: {
          user_id: Mock.mock('@guid'),
          auth_token: Mock.mock('@guid'),
          real_name: 'Admin',
          auths: {}
        },
        code: 0,
        msg: 'Login success'
      }
    }
    return {
      error: {
        code: 4000,
        msg: 'Your account username or password is incorrect'
      }
    }
  })

  // 菜单获取
  Mock.mock(/\/getUserModuleList/, {
    data: [{
        path: "/test",
        name: "Test",
        title: 'Test',
        icon: "md-document",
        compName: "Test",
        compPath: "/Test"
      },
      {
        path: '/hello',
        name: 'Hello',
        title: 'Hello',
        icon: 'md-chatbubbles',
        children: [{
          path: '/hello/hello-world',
          name: 'HelloWorld',
          title: 'HelloWorld',
          icon: 'md-text',
          compName: 'HelloWorld',
          compPath: '/hello/helloWorld'
        }]
      }, {
        path: '/manage',
        name: 'Manage',
        title: '系统管理',
        icon: 'md-folder-open',
        children: [{
          path: '/manage/users',
          title: '用户管理',
          name: 'Users',
          icon: 'md-person',
          compName: 'Users',
          compPath: '/manage/users'
        }]
      }
    ],
    code: 0,
    msg: 'Get menu success'
  })

  // 密码修改
  Mock.mock(/\/edit-password/, config => {
    const {
      currentPassword
    } = qs.parse(config.body)
    if (currentPassword === 'wasd@007') {
      return {
        error: {
          code: 0,
          msg: 'Edit password success'
        }
      }
    }
    return {
      error: {
        code: 4000,
        msg: 'Your current password is incorrect'
      }
    }
  })
}
