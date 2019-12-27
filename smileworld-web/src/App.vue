<template>
  <div id="app">
    <transition name="fade" mode="out-in">
      <router-view />
    </transition>
  </div>
</template>

<script>
export default {
  name: 'App'
}
</script>

<style lang="postcss">
#app {
  // 过渡动画
  & .fade-enter-active,
  & .fade-leave-active {
    transition: opacity 0.3s;
  }
  & .fade-enter,
  & .fade-leave-active {
    opacity: 0;
  }
}
// 自定义滚动条样式
::-webkit-scrollbar-track-piece {
  width: 13px;
  background-color: #f8f8f8;
}
::-webkit-scrollbar {
  width: 13px;
  height: 13px;
}
::-webkit-scrollbar-thumb {
  height: 50px;
  cursor: pointer;
  background-color: #e6e6e6;
}
</style>
