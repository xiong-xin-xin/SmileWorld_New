<template>
  <div id="content">
    <Card :bordered="false" dis-hover>
      <div class="search-bar" ref="extra">
        <slot name="search"> </slot>
        <!-- search -->
        <slot name="extra"> </slot>
        <!-- extra -->
      </div>
      <!-- .search-bar -->
      <slot> </slot>
    </Card>
  </div>
</template>
<script>
export default {
  name: 'Content',
  data: () => ({
  }),
  updated() {
    const auths = this.$store.state.app.userInfo.auths[this.$parent.$options.name];
    if (auths !== undefined) {
      var toolbar = this.$slots.extra[0].elm.children;
      for (var em of toolbar) {
        if (em.type == "button") {
          if (auths.indexOf(em.innerText.trim()) != -1) {
            em.style.display = "inline";
          }
        }
      }
    }
  },
}
</script>
<style lang="postcss" >
.search-bar {
  display: flex;
  justify-content: space-between;
}
</style>
