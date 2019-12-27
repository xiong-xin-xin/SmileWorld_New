import Mock from 'mockjs'
import qs from 'qs'

export default () => {

  let columns = [{
      type: 'selection',
      width: 60,
      align: 'center',
      fixed: 'left'
    },
    {
      type: 'index',
      width: 60,
      align: 'center'
    },
    {
      title: '媒体类别',
      key: 'mediaType',
      sortable: true
    },
    {
      title: '媒体级别',
      key: 'mediaLevel',
      sortable: true
    },
    {
      title: '媒体地区',
      key: 'mediaCity',
      sortable: true
    },
    {
      title: 'vivo交接人',
      key: 'vivoHandover',
      sortable: true
    },
    {
      title: '媒体名称',
      key: 'mediaName',
      sortable: true
    },
    {
      title: '媒体交接人',
      key: 'mediaHandover',
      sortable: true
    },
    {
      title: '性别',
      key: 'gender',
      sortable: true
    },
    {
      title: '生日',
      key: 'birth',
      sortable: true
    },
    {
      title: '电话',
      key: 'mobile',
      sortable: true
    },
    {
      title: '微信',
      key: 'weChat',
      sortable: true
    },
    {
      title: '邮箱',
      key: 'email',
      sortable: true
    },
    {
      title: '地址',
      key: 'address',
      sortable: true
    },
    {
      title: '备注',
      key: 'remarks',
      sortable: true
    }
  ]

  let newsUsers = [{
      id: '0',
      mediaType: '新闻类',
      mediaLevel: 'A级',
      mediaCity: '东莞',
      vivoHandover: '张三',
      mediaName: '新浪',
      mediaHandover: '王五',
      gender: 1,
      birth: '1996-05-61',
      mobile: '1514456446',
      weChat: 'xxxx',
      email: 'XASDS@QQ.COM',
      address: 'cxxxx路xx街道',
      testBarcode: '12123123123',
      remarks: '备注'
    },
    {
      id: '1',
      mediaType: '娱乐类',
      mediaLevel: 'A级',
      mediaCity: '东莞',
      vivoHandover: '张三',
      mediaName: '新浪',
      mediaHandover: '王五',
      gender: 1,
      birth: '1996-05-61',
      mobile: '1514456446',
      weChat: 'xxxx',
      email: 'XASDS@QQ.COM',
      address: 'cxxxx路xx街道',
      testBarcode: '12123123123',
      remarks: '备注'
    },
    {
      id: '2',
      mediaType: '科技类',
      mediaLevel: 'A级',
      mediaCity: '东莞',
      vivoHandover: '张三',
      mediaName: '新浪',
      mediaHandover: '王五',
      gender: 1,
      birth: '1996-05-61',
      mobile: '1514456446',
      weChat: 'xxxx',
      email: 'XASDS@QQ.COM',
      address: 'cxxxx路xx街道',
      testBarcode: '12123123123',
      remarks: '备注'
    }
  ]

  // 用户列表
  Mock.mock(/\/conference\/getNewsUserPageList/, config => {
    const {
      pagination,
      name
    } = qs.parse(config.url.split('?')[1])
    let {
      pageIndex,
      pageSize
    } = JSON.parse(pagination)
    columns.push();

    let _users = newsUsers.filter(u => {
      if (name && u.mediaHandover.indexOf(name) === -1) {
        return false
      }
      return true
    })
    const total = _users.length
    const pageMax = Math.ceil(total / pageSize)
    pageIndex = pageIndex > pageMax ?
      pageMax :
      pageIndex
    _users = _users.filter((u, index) => index < pageSize * pageIndex && index >= pageSize * (pageIndex - 1))
    return {
      data: {
        total: total,
        data: _users,
        columns: columns
      },
      code: 0,
      msg: 'Get users success'
    }
  })
  // 编辑用户
  Mock.mock(/\/conference\/editNewsUser/, config => {
    const {
      mediaType,
      mediaLevel,
      mediaName,
      mediaHandover,
      vivoHandover,
      mobile,
      id,
      email,
      birth,
      mediaCity,
      gender
    } = qs.parse(config.body)

    var result = newsUsers.some(u => {
      if (u.id === id) {
        u.mediaType = mediaType
        u.mediaLevel = mediaLevel
        u.mediaName = mediaName
        u.mediaHandover = mediaHandover
        u.vivoHandover = vivoHandover
        u.mobile = mobile
        u.email = email
        u.birth = birth
        u.mediaCity = mediaCity
        u.gender = parseInt(gender)
        return true;
      }
    })
    if (!result) {
      newsUsers.unshift({
        id: Mock.mock('@guid'),
        mediaType: mediaType,
        mediaLevel: mediaLevel,
        mediaName: mediaName,
        mediaHandover: mediaHandover,
        vivoHandover: vivoHandover,
        mobile: mobile,
        email: email,
        birth: birth,
        mediaCity: mediaCity,
        gender: parseInt(gender)
      })
    }
    return {
      data: '',
      code: 0,
      msg: 'Update success'
    }
  })


  // 导入副本
  Mock.mock(/\/conference\/importDuplicate/, config => {




  })
}
