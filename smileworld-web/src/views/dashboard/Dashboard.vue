<template>
  <Content id="dashboard">
    <h1>{{ msg }}</h1>
    <h3>基于 Vue.js 的开源集成方案</h3>
    <h3>
      一分耕耘，一分收获。一艺之成，当尽毕生之力。
    </h3>
    <img class="logo" src="@/assets/logo1.png" alt="logo">
    <Col :xs="0" :md="24" type="flex" justify="end" style="position: absolute;top:10px;right:10px;text-align: right">
    <div>
      <iframe
        src="//www.seniverse.com/weather/weather.aspx?uid=UC4AB25912&cid=CHBJ000000&l=zh-CHS&p=SMART&a=1&u=C&s=1&m=0&x=1&d=3&fc=&bgc=&bc=&ti=0&in=0&li="
        frameborder="0" scrolling="no" width="200" height="230" allowTransparency="true"></iframe>
    </div>
    </Col>
    <div style="width:250px; height:250px; ;background-color:transparent;position: absolute;top:10px;right:10px"> </div>
  </Content>
</template>
<script>
export default {
  name: "Dashboard",
  data: () => ({
    msg: "smile world"
  }),
  mounted() {},
  methods: {}
};
</script>
<!-- Add "scoped" attribute to limit CSS to this component only -->
<style lang="postcss" scoped>
#dashboard {
  text-align: center;
  & h1 {
    margin-bottom: 10px;
    font-size: 20px;
  }
  & h3 {
    margin: 20px 0;
    font-weight: 400;
    & a {
      color: #51b883;
    }
  }
}
</style>
