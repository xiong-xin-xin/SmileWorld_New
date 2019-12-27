import config from '@/config'
const signalR = require('../../node_modules/@aspnet/signalr');


/*服务器调用客户端方法*/

// 多账号登陆退出框提示
export const signOut = {
  name: 'SignOut',
  method: () => {}
}
//强制退出
export const forceSignOut = {
  name: 'ForceSignOut',
  method: () => {}
}
export const setOnlineNumber = {
  name: 'SetOnlineNumber',
  method: () => {}
}


//客户端所有的方法
const clientMethodSets = [signOut, forceSignOut, setOnlineNumber]; //将需要注册的方法放进集合
// 建立连接
export async function startConnection() {
  var connection = new signalR.HubConnectionBuilder()
    .withUrl(`${config.baseURL.replace('api','').replace('/','')}/hub`, {
      accessTokenFactory: () => JSON.parse(sessionStorage.getItem('userInfo')).auth_token.token
    }).build();

  clientMethodSets.map((item) => {
    connection.on(item.name, item.method)
  })
  await connection.start();
  return connection;
}
