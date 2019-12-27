<template>
  <div id="login-warp">
    <div id="login">
      <transition :name="settings.visible ? 'slideleft' : 'slideright'">
        <IConfig v-if="settings.visible" />
        <!-- IConfig -->
        <div class="login" v-if="!settings.visible">
          <div class="logo-info">
            <img class="logo" src="@/assets/logo.png" alt="logo">
            <p class="name">smile world
              <br>
              <span class="sub">基于 Vue.js 的方案</span>
            </p>
          </div>
          <!-- .logo-info -->
          <IForm ref="login" :model="login" :elem="loginElem" :rules="loginRule" :btn-loading="$store.state.app.loading"
            btn-long submit-text="Sign in" @on-submit="handleLogin" />
          <!-- IForm -->
          <p class="version">Version:{{ version }}</p>
          <!-- .version -->
        </div>
      </transition>
      <a v-if="settings.button" class="settings" href="#" @click.prevent="settings.visible = !settings.visible">
        <Icon :type="settings.visible ? 'md-laptop' : 'md-construct'" size="16" style="margin-top: -2px;" />
        <strong>{{ settings.visible ? 'Login' : 'Setting'}}</strong>
      </a>
    </div>
    <!-- #login -->
    <IError />
    <!-- IError -->
  </div>
</template>
<script>
import config from "@/config";
import ax from "@/utils/axios";
import { login } from "@/services/app";
import IConfig from "@/layouts/partials/Config";
import IError from "@/layouts/partials/Error";
export default {
  name: "Login",
  components: {
    IConfig,
    IError
  },
  data: () => ({
    // 设置属性对象
    settings: {
      button: false,
      visible: false
    },
    // 版本属性
    version: "",
    // 表单数据对象(登录)
    login: {
      user: "",
      pwd: ""
    },
    // 表单元素数组(登录)
    loginElem: [
      {
        prop: "user",
        placeholder: "Username",
        prefix: "md-contact"
      },
      {
        prop: "pwd",
        placeholder: "Password",
        prefix: "md-lock",
        type: "password"
      }
    ],
    // 表单验证规则(登录)
    loginRule: {
      user: [
        {
          required: true,
          message: "Please fill in the username",
          trigger: "blur"
        }
      ],
      pwd: [
        {
          required: true,
          message: "Please fill in the password",
          trigger: "blur"
        }
      ]
    }
  }),
  mounted() {
    const { env, version } = config;
    switch (env) {
      case "release":
        this.version = `${version} Release`;
        break;
      case "production":
        // this.version = `${version} Production`
        this.version = version;
        break;
      default:
        this.settings.button = true;
        this.login = {
          user: "admin",
          pwd: "123456"
        };
        this.version =
          env === "development" ? `${version} Develop` : `${version} Test`;
    }
    this.$store.commit("TITLE", "Login");
  },
  methods: {
    // 用户登录
    handleLogin() {
      // 配置默认接口地址
      const baseURL = localStorage.getItem("newBaseAPI") || config.baseURL;
      if (config.env !== "production") {
        console.log("API_URL", baseURL);
      }
      ax.defaults.baseURL = baseURL;

      this.$Loading.start();
      this.$store.commit("LOADING", true);
      // 模拟异步请求
      setTimeout(() => {
        login({ username: this.login.user, password: this.login.pwd })
          .then(res => {
            this.$Loading.finish();
            this.$store.commit("LOADING", false);
            const userInfo = res.data;
            if (userInfo) {
              ax.defaults.headers.common["Authorization"] =
                "Bearer " + userInfo.auth_token.token;

              // 获取用户信息
              this.$store.commit("USER_INFO", userInfo);
              // 获取菜单列表
              this.$store.dispatch("handleMenu");
            } else {
              this.$Message.error("该用户登陆无效！");
            }
          })
          .catch(() => {
            this.$Loading.error();
            this.$store.commit("LOADING", false);
          });
      }, 800);
    }
  }
};
</script>
<style lang="postcss" scoped>
#login {
  border-radius: 5px;
  -moz-border-radius: 5px;
  background-clip: padding-box;
  margin: auto;
  width: 360px;
  height: 350px;
  margin-top: 175px;
  padding: 35px 35px 15px 35px;
  background: #fff;
  border: 1px solid #eaeaea;
  box-shadow: 0 0 25px #cac6c6;
  position: relative;
  /* 过渡动画 */
  & .slideleft-enter-active,
  & .slideright-enter-active {
    transform: translateX(0);
    transition: all 0.5s;
  }
  & .slideleft-enter,
  & .slideright-enter {
    opacity: 0;
  }
  & .slideleft-leave-active,
  & .slideright-leave-active {
    opacity: 0;
    transition: all 0.5s;
  }
  & .slideleft-leave,
  & .slideright-leave {
    transform: translateX(0);
  }
  & .slideleft-enter,
  & .slideright-leave-active {
    transform: translateX(40px);
  }
  & .slideleft-leave-active,
  & .slideright-enter {
    transform: translateX(-40px);
  }
  /* end */
  & .settings {
    bottom: 38px;
    position: absolute;
  }
  & .login {
    position: absolute;
    width: 288px;
  }
  & .logo-info {
    height: 64px;
    margin-bottom: 22px;
    text-align: center;
    & .logo {
      height: 60px;
      margin-right: 10px;
      width: 130px;
    }
    & .name {
      bottom: 14px;
      display: inline-block;
      font-size: 18px;
      line-height: 20px;
      position: relative;
      text-align: left;
    }
    & .sub {
      font-size: 12px;
    }
  }
  & .version {
    color: #888;
    text-align: center;
  }
}
#login-warp {
  margin: 0;
  position: absolute;
  left: 0;
  top: 0;
  right: 0;
  bottom: 0;
  background: url(../assets/loginbck.ed4cec20.png) no-repeat 0 0;
  background-repeat: no-repeat;
  background-size: cover;
  width: 100%;
  height: 100%;
}
</style>
