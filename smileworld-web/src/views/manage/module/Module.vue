<template>
  <div>
    <Card>
      <div>
        <template>
          <Row>
            <Col span="15" class="margin-bottom-10 oprbth">
            <Button type="info" @click="openAddModal(1)">
              <Icon type="md-add"></Icon>&nbsp;添加资源
            </Button>
            <Button :disabled="setting.loading" type="success" @click="getData">
              <Icon type="md-refresh"></Icon>&nbsp;刷新数据
            </Button>
            <Tooltip placement="top">
              <i-switch size="large" v-model="props.isFold">
                <span slot="open">开启</span>
                <span slot="close">关闭</span>
              </i-switch>
              <div slot="content">
                <p>是否折叠所有的节点</p>
              </div>
            </Tooltip>
            </Col>
          </Row>
          <zk-table ref="table" sum-text="sum" index-text="#" :data="datas" :columns="columns" :stripe="props.stripe"
            :border="props.border" :show-header="props.showHeader" :show-summary="props.showSummary"
            :show-row-hover="props.showRowHover" :show-index="props.showIndex" :tree-type="props.treeType"
            :is-fold="!props.isFold" :expand-type="props.expandType" :selection-type="props.selectionType">
            <template slot="type" slot-scope="scope">
              <span v-if="scope.row.isMenu===1">
                <Icon type="md-apps"></Icon>&nbsp;菜单
              </span>
              <span v-else>
                <Icon type="ios-disc"></Icon>&nbsp;按钮
              </span>
            </template>
            <template slot="icon" slot-scope="scope">
              <span v-if="scope.row.icon!=null && scope.row.icon!=''">
                <Icon :type="scope.row.icon"></Icon>
              </span>
              <span v-else>-</span>
            </template>
            <template slot="enabled" slot-scope="scope">
              <Tag v-if="scope.row.enabled==true" color="success">启 用</Tag>
              <Tag v-else color="error">禁 用</Tag>
            </template>
            <template slot="action" slot-scope="scope">
              <Button type="primary" @click="openAddModal(2,scope.row)" size="small">编辑</Button>
              <Button type="success" @click="openAddModal(3,scope.row)" size="small"
                style="margin-left: 5px;">添加下级</Button>
              <Button type="error" @click="removeModalOpen(scope.row.id)" size="small"
                style="margin-left: 5px;">删除</Button>
            </template>
          </zk-table>
        </template>
      </div>
    </Card>
    <ModuleEdit ref="edit" :model="edit" @on-update="getData" />
    <Modal v-model="removeModal" width="360">
      <p slot="header" style="color:#f60;text-align:center">
        <Icon type="information-circled"></Icon>
        <span>提示</span>
      </p>
      <div style="text-align:center">
        <p>此操作为不可逆操作，是否确认删除？</p>
      </div>
      <div slot="footer">
        <Button type="error" size="large" long @click="handleDelete">确认删除</Button>
      </div>
    </Modal>
  </div>
</template>
<script>
import { delModule, getModuleList } from "@/services/manage/module";
import ModuleEdit from "./ModuleEdit";
export default {
  name: "Module",
  components: { ModuleEdit },
  data() {
    return {
      removeModal: false,
      setting: {
        loading: true,
        showBorder: true
      },
      props: {
        stripe: true,
        border: true,
        showHeader: true,
        showSummary: false,
        showRowHover: true,
        showIndex: false,
        treeType: true,
        isFold: true,
        expandType: false,
        selectionType: false
      },
      datas: [],
      columns: [
        { label: "资源名称", prop: "title", width: "200px" },
        {
          label: "类型",
          prop: "isMenu",
          type: "template",
          template: "type",
          width: "80px"
        },
        { label: "资源链接", prop: "path", width: "250px" },
        { label: "Name", prop: "name", width: "150px" },
        { label: "Api", prop: "api", width: "150px" },
        {
          label: "状态",
          prop: "enabled",
          type: "template",
          template: "enabled",
          width: "77px"
        },
        {
          label: "图标",
          prop: "icon",
          type: "template",
          template: "icon",
          width: "80px"
        },
        { label: "排序", prop: "orderSort", width: "50px" },
        { label: "说明", prop: "description" },
        {
          label: "操作",
          type: "template",
          prop: "action",
          template: "action",
          width: "300px",
          fixed: "right"
        }
      ],
      edit: {},
      removeObject: null
    };
  },
  created() {
    this.getData();
  },
  methods: {
    async getData() {
      this.setting.loading = true;
      getModuleList({
        ...this.search,
        pagination: {
          pageIndex: 1,
          pageSize: 200
        }
      }).then(res => {
        this.setting.loading = false;
        const { data } = res.data;
        this.datas = data;
      });
    },
    removeModalOpen(id) {
      this.removeModal = true;
      this.removeObject = id;
    },
    async handleDelete() {
      this.setting.loading = true;
      this.$Message.loading({
        content: "资源删除中...",
        duration: 0
      });
      delModule({
        ids: [this.removeObject]
      })
        .then(() => {
          this.getData(false);
          this.$Message.destroy();
          this.$Message.success({
            content: "资源删除成功",
            duration: 1.5
          });
        })
        .catch(() => {
          this.$Loading.error();
        });
      this.setting.loading = false;
      this.removeModal = false;
    },
    openAddModal(type, row) {
      this.$refs["edit"].handleModal(type); // 显示模态框
      this.edit = { enabled: 1, parentId: 0 };
      if (type == 2) {
        this.edit = { ...row };
      } else if (type == 3) {
        //添加下级按钮
        this.edit = {
          enabled: 1,
          isMenu: 2,
          parentName: row.title,
          id: "",
          parentId: row.id,
          name: row.name,
          orderSort: 1
        };
      }
    }
  }
};
</script>
<style>
.zk-table__header-wrapper {
  width: calc(100% - 13px);
}
.zk-table__body-wrapper {
  overflow-y: scroll;
  max-height: 450px;
}
</style>
