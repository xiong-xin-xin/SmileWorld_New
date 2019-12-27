<template>
  <div id="footer">
    <p class="copyright"> Copyright &copy; 2018-2019 </p>
  </div>
</template>
<script>
export default {
  name: 'IFooter'
}
</script>
<style lang="postcss" scoped>
#footer {
  background-color: #fff;
  color: #666;
  overflow: hidden;
  width: 100%;
  text-align: center;
  & .copyright {
    margin: 10px 0;
  }
  position: absolute;
  bottom: 0px;
}
</style>
