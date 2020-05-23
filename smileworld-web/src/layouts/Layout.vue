<template>
  <div id="layout">
    <IHeader />
    <!-- IHeader -->
    <Row>
      <Col :xs="0" :sm="0" :md="4" :lg="3">
      <ISidebar ref="sidebar" />
      <!-- ISidebar -->
      </Col>
      <Col :xs="24" :sm="24" :md="20" :lg="21">
      <ITabs />
      <!-- ITabs -->
      <div id="container">
        <div id="main">
          <IBreadcrumb />

          <keep-alive>
            <router-view v-if="keepAlive" />
          </keep-alive>
          <router-view v-if="!keepAlive" />
        </div>
        <!-- #main -->
        <IFooter />
        <!-- IFooter -->
      </div>
      <!-- #container -->
      </Col>
    </Row>
    <IError />
    <!-- IError -->
  </div>
</template>

<script>
import IHeader from "@/layouts/partials/Header";
import ISidebar from "@/layouts/partials/Sidebar";
import ITabs from "@/layouts/partials/Tabs";
import IBreadcrumb from "@/layouts/partials/Breadcrumb";
import IFooter from "@/layouts/partials/Footer";
import IError from "@/layouts/partials/Error";
import { mapGetters } from "vuex";
import store from "@/store";
import {
  startConnection,
  signOut,
  forceSignOut,
  setOnlineNumber
} from "@/utils/signalr";
import { get } from "@/utils/axios";
import { setTimeout } from "timers";
//store.dispatch("handleMenu");
export default {
  name: "ILayout",
  components: { IHeader, ISidebar, ITabs, IBreadcrumb, IFooter, IError },
  data: () => ({
    routes: [],
    keepAlive: true,
    connection: ""
  }),
  computed: {
    ...mapGetters({
      menu: "getMenu",
      tabs: "getTabs"
    })
  },
  watch: {
    // 侦听路由变化
    $route() {
      this.$store.commit("MENU_SELECT", this.$route.fullPath); // 选择菜单
      this.$refs["sidebar"].updateOpened(); // 手动更新展开的子菜单
      // 设置路由组件缓存
      for (const route of this.routes) {
        if (route.path === this.$route.path) {
          this.keepAlive = route.meta.keepAlive;
        }
      }
    },
    // 侦听标签变化
    tabs() {
      // 根据 tabs 的增减来判断路由组件是否进行缓存
      this.handleKeepAlive();
    }
  },
  async mounted() {
    this.routes = this.$router["options"]["routes"][0]["children"]; // 获取动态路由
    // 根据 tabs 的增减来判断路由组件是否进行缓存
    this.handleKeepAlive();
    //提醒另一个用户已登录
    if (this.connection !== "") this.connection.stop();
    signOut.method = message => {
      this.$Modal.confirm({
        title: "提示",
        content: message,
        onOk: () => {
          this.connection.invoke("ForceSignOut");
        },
        onCancel: () => {
          store.commit("MENU_RESET"); // 重置菜单
          window.location.reload();
        }
      });
    };
    forceSignOut.method = () => {
      store.commit("MENU_RESET");
      window.location.reload();
    };
    setOnlineNumber.method = number => {
      console.log(`当前在线人数：${number}`);
    };
    if (process.env.VUE_APP_ENV !== "development") {
      this.connection = await startConnection();
      this.connection.invoke("SignIn");
    }
  },
  methods: {
    // 根据 tabs 的增减来判断路由组件是否进行缓存
    handleKeepAlive() {
      for (const route of this.routes) {
        if (route.meta.keepAliveUse) {
          route.meta.keepAlive = false;
        }
        for (const tab of this.tabs) {
          if (route.path === tab.path) {
            route.meta.keepAliveUse = true;
            route.meta.keepAlive = true;
          }
        }
      }
    }
  }
};
</script>
<style lang="postcss">
.ivu-modal-body {
  max-height: 800px !important;
  overflow: auto !important;
}

#container {
  background-color: #f0f0f0;
  height: calc(100vh - 100px);
  overflow: auto;
  & #main {
    min-height: calc(100% - 46px);
    overflow: hidden;
    padding: 10px 16px;
  }

  /* fade-transform */
  & .fade-transform-enter-active,
  & .fade-transform-leave-active {
    transition: all 0.5s;
  }

  & .fade-transform-enter {
    opacity: 0;
    transform: translateX(-30px);
  }

  & .fade-transform-leave-to {
    opacity: 0;
    transform: translateX(30px);
  }
}
::selection {
  background: #bcee68;
  color: #333;
}
.tl {
  text-align: left;
}
.tc {
  text-align: center;
}
.tr {
  text-align: right;
}
.toolbar {
  margin-bottom: 10px;
  text-align: right;
  & .number {
    margin-right: 8px;
  }
  & .ivu-btn {
    margin-top: 2px;
    margin-right: 5px !important;
  }
}
.oprbth > .ivu-btn {
  margin-right: 2px !important;
}
</style>
