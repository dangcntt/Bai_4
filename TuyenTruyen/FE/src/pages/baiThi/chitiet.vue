<script>
import Layout from "@/layouts/main";
import PageHeader from "@/components/page-header";
import {numeric, required} from "vuelidate/lib/validators";
import appConfig from "@/app.config";
import {pagingModel} from "@/models/pagingModel";
import Multiselect from "vue-multiselect";
import {lichSuModel} from "@/models/lichSuModel";
import DatePicker from "vue2-datepicker";
import Treeselect from "@riophae/vue-treeselect";
export default {
  page: {
    title: "Quản lý tuyên truyền",
    meta: [{name: "description", content: appConfig.description}],
  },
  components: {Layout, PageHeader, DatePicker},
  data() {
    return {
      title: "Quản lý tuyên truyền",
      items: [
        {
          text: "Khách hàng",
          href: '/bai-thi'
        },
        {
          text: "Danh sách",
          active: true,
        }
      ],
      data: [],
      fields: [
        { key: 'STT',
          label: 'STT',
          class: 'td-stt',
          sortable: false,
          thStyle: "text-align:center",
        },
        {
          key: "name",
          label: "Tiêu đề",
          class: 'td-name',
          sortable: false,
          thStyle: "text-align:center",
        },
        {
          key: "noiDungTuyenTruyen",
          label: "Nội dung tuyên truyền",
          class: 'td-ten',
          sortable: false,
          thStyle: "text-align:center",
        },
        {
          key: "ngayTuyenTruyenShow",
          label: "Ngày tuyên truyền",
          class: 'td-taikhoan',
          thStyle: "text-align:center",
          sortable: false,
        },
        {
          key: "soLanPhat",
          label: "Số lần phát",
          class: 'td-diemSo',
          thStyle: "text-align:center",
          sortable: true,
          thClass: 'hidden-sortable'
        },
        {
          key: 'process',
          label: 'Xử lý',
          class: 'td-xuly',
          sortable: false,
        }
      ],
      currentPage: 1,
      perPage: 10,
      pageOptions: [5, 10, 25, 50, 100],
      showModal: false,
      showModalIsPublish: false,
      showDeleteModal: false,
      submitted: false,
      sortBy: "age",
      filter: null,
      sortDesc: false,
      filterOn: [],
      numberOfElement: 1,
      totalRows: 1,
      model: lichSuModel.baseJson(),
      pagination: pagingModel.baseJson(),
    };
  },
  created() {
  },
  methods: {
    async fnGetList() {
      this.$refs.tblList?.refresh()
    },
    async handleDelete() {
      if (this.model.id != 0 && this.model.id != null && this.model.id) {
        await this.$store.dispatch("lichSuStore/delete",{id :  this.model.id}).then((res) => {
          if (res.code===0) {
            this.fnGetList();
            this.showDeleteModal = false;
          }
          var a = {
            message: res.message,
            code: res.code
          };
          //   console.log("LOG A : " ,a)
          this.$store.dispatch("snackBarStore/addNotify", {
            message: res.message,
            code: res.code
          });
        });
      }
    },
    handleShowDeleteModal(id) {
      this.model.id = id;
      this.showDeleteModal = true;
    },
    async handleSubmit(e) {
      e.preventDefault();
      this.submitted = true;

        let loader = this.$loading.show({
          container: this.$refs.formContainer,
        });
        if (
            this.model.id != 0 &&
            this.model.id != null &&
            this.model.id
        ) {
          this.model.idBaiThi = this.$route.params.id;
          console.log("LOG USER UPDATE NE : " , this.model)
          // Update model
          await this.$store.dispatch("lichSuStore/update", this.model).then((res) => {
            if (res.code === 0) {
              this.showModal = false;
              this.model = lichSuModel.baseJson();
              this.$refs.tblList.refresh();
            }
            this.$store.dispatch("snackBarStore/addNotify", {
              message: res.message,
              code: res.code,
            });
          });
        } else {
          // Create model
          this.model.idBaiThi = this.$route.params.id;
          console.log("LOG USER CREATE NE : " , this.model)
          await this.$store.dispatch("lichSuStore/create", this.model).then((res) => {
            if (res.code === 0) {
              this.fnGetList();
              this.showModal = false;
              this.model = lichSuModel.baseJson();
            }
            this.$store.dispatch("snackBarStore/addNotify", {
              message: res.message,
              code: res.code,
            });
          });
        }
        loader.hide();
      this.submitted = false;
    },
    async handleUpdate(id) {
      await this.$store.dispatch("lichSuStore/getById", {id : id}).then((res) => {
        if (res.code===0) {
          console.log(res)
          this.model = lichSuModel.fromJson(res.data);
          this.showModal = true;
        } else {
          this.$store.dispatch("snackBarStore/addNotify", {
            message: res.message,
            code: res.code,
          });
        }
      });
    },
    async handlePublish(id) {
      await this.$store.dispatch("lichSuStore/getById", {id : id}).then((res) => {
        if (res.code===0) {
          this.model = lichSuModel.fromJson(res.data);
          this.showModalIsPublish = true;
        } else {
          this.$store.dispatch("snackBarStore/addNotify", {
            message: res.message,
            code: res.code,
          });
        }
      });
    },
    async handleSubmitIsPublish(e) {
      e.preventDefault();
      this.submitted = true;

        let loader = this.$loading.show({
          container: this.$refs.formContainer,
        });
        if (
            this.model.id != 0 &&
            this.model.id != null &&
            this.model.id
        ) {
          this.model.isDuyet = true
          // Update model
          await this.$store.dispatch("lichSuStore/update", this.model).then((res) => {
            if (res.code === 0) {
              this.showModalIsPublish = false;
              this.model = lichSuModel.baseJson();
              this.$refs.tblList.refresh();
            }
            this.$store.dispatch("snackBarStore/addNotify", {
              message: res.message,
              code: res.code,
            });
          });
        }
        loader.hide();
      this.submitted = false;
    },
    myProvider (ctx) {
      const params = {
        start: ctx.currentPage,
        limit: ctx.perPage,
        content: this.filter,
        sortBy: ctx.sortBy,
        sortDesc: ctx.sortDesc,
        idBaiThi: this.$route.params.id,
      }
      this.loading = true
      try {
        let promise =  this.$store.dispatch("lichSuStore/getPagingParamsLS", params)
        return promise.then(resp => {
          let items = resp.data.data
          this.totalRows = resp.data.totalRows
          this.numberOfElement = resp.data.data.length
          this.loading = false
          return items || []
        })
      } finally {
        this.loading = false
      }
    },
  
  },
  mounted() {
  },
  watch: {
    model: {
      deep: true,
      handler(val) {
        // addCoQuanToModel()
        // this.saveValueToLocalStorage()
      }
    },
    showModal(status) {
      if (status == false) this.model = lichSuModel.baseJson();
    },
    // model() {
    //   return this.model;
    // },
    showDeleteModal(val) {
      if (val == false) {
        this.model.id = null;
      }
    }
  },
};
</script>

<template>
  <Layout>
    <PageHeader :title="title" :items="items"/>
    <div class="row">
      <div class="col-12">
        <div class="card">
          <div class="card-body">
            <div class="row mb-2">
              <div class="col-sm-4">
                <div class="search-box me-2 mb-2 d-inline-block">
                  <div class="position-relative">
                    <input
                        v-model="filter"
                        type="text"
                        class="form-control"
                        placeholder="Tìm kiếm ..."
                    />
                    <i class="bx bx-search-alt search-icon"></i>
                  </div>
                </div>
              </div>
              <div class="col-sm-8">
                <div class="text-sm-end">
                  <b-button
                      type="button"
                      class="btn-label btn-add mb-2 me-2"
                      @click="showModal = true" size="sm"
                  >
                    <i class="mdi mdi-plus me-1 label-icon"></i> Thêm
                  </b-button>
                  <b-modal
                      v-model="showModal"
                      title="Thông tin tuyên truyền"
                      title-class="text-black font-18"
                      body-class="p-3"
                      hide-footer
                      centered
                      no-close-on-backdrop
                      size="lg"
                  >
                    <form @submit.prevent="handleSubmit"
                          ref="formContainer"
                    >
                      <div class="row">
                        <div class="col-md-12">
                          <div class="mb-3">
                            <label class="text-left">Tiêu đề</label>
                            <span style="color: red">&nbsp;*</span>
                            <input type="hidden" v-model="model.id"/>
                            <input
                                id="userName"
                                v-model.trim="model.name"
                                type="text"
                                class="form-control"
                                placeholder="Nhập tiêu đề"
                            />
                          </div>
                        </div>
                        <div class="col-md-12">
                          <div class="mb-3">
                            <label class="text-left">Nội dung tuyên tuyền</label>
                            <span style="color: red">&nbsp;*</span>
                            <input type="hidden" v-model="model.id"/>
                            <input
                                id="lastName"
                                v-model="model.noiDungTuyenTruyen"
                                type="text"
                                class="form-control"
                                placeholder="Nhập nội dung"
                            />
                          </div>
                        </div>
                        
                        <div class="col-md-6">
                          <div class="mb-3">
                            <label class="text-left">Ngày tuyên truyền</label>
                            <span style="color: red">&nbsp;*</span>
                            <date-picker
                                v-model="model.ngayTuyenTruyen"
                                format="DD/MM/YYYY"
                                id="percent"
                                placeholder="Nhập ngày tuyên truyền"
                            >
                            </date-picker>
                          </div>
                        </div>
                        <div class="col-md-6">
                          <div class="mb-3">
                            <label class="text-left">Số lần phát</label>
                            <span style="color: red">&nbsp;*</span>
                            <input type="hidden" v-model="model.id"/>
                            <input
                                id="lastName"
                                v-model="model.soLanPhat"
                                type="number"
                                class="form-control"
                                placeholder="Nhập mô tả"
                            />
                          </div>
                        </div>

                      </div>
                      <div class="text-end pt-2 mt-3">
                        <b-button variant="light" @click="showModal = false" class="border-0">
                          Đóng
                        </b-button>
                        <b-button  type="submit" variant="success" class="ms-1 btn-add">Lưu
                        </b-button>
                      </div>
                    </form>
                  </b-modal>
                  <b-modal
                      v-model="showModalIsPublish"
                      title="Thông tin tuyên truyền"
                      title-class="text-black font-18"
                      body-class="p-3"
                      hide-footer
                      centered
                      no-close-on-backdrop
                      size="lg"
                  >
                    <form @submit.prevent="handleSubmitIsPublish"
                          ref="formContainer"
                    >
                      <div class="row">
                        <div class="col-md-12">
                          <div class="mb-3">
                            <label class="text-left">Tiêu đề</label>
                            <span style="color: red">&nbsp;*</span>
                            <input type="hidden" v-model="model.id"/>
                            <input
                                id="userName"
                                v-model.trim="model.name"
                                type="text"
                                class="form-control"
                                placeholder="Nhập tiêu đề"
                                :disabled="true"
                            />
                          </div>
                        </div>
                        <div class="col-md-12">
                          <div class="mb-3">
                            <label class="text-left">Tên đơn vị</label>
                            <span style="color: red">&nbsp;*</span>
                            <input type="hidden" v-model="model.id"/>
                            <input
                                id="lastName"
                                v-model="model.tenDonVi"
                                type="text"
                                class="form-control"
                                placeholder="Nhập tên đơn vị"
                                :disabled="true"
                            />
                          </div>
                        </div>
                        <div class="col-md-12">
                          <div class="mb-3">
                            <label class="text-left">Mô tả</label>
                            <span style="color: red">&nbsp;*</span>
                            <input type="hidden" v-model="model.id"/>
                            <textarea
                                id="lastName"
                                v-model="model.moTa"
                                type="text"
                                class="form-control"
                                placeholder="Nhập mô tả"
                                :disabled="true"
                            >
                            </textarea>
                          </div>
                        </div>
                        <div class="col-md-6">
                          <div class="mb-3">
                            <label class="text-left">Ngày bắt đầu</label>
                            <span style="color: red">&nbsp;*</span>
                            <date-picker
                                v-model="model.ngayBatDauTuyenTruyen"
                                format="DD/MM/YYYY"
                                id="percent"
                                placeholder="Nhập ngày bắt đầu"
                                :disabled="true"
                            >
                            </date-picker>
                          </div>
                        </div>
                        <div class="col-md-6">
                          <div class="mb-3">
                            <label class="text-left">Ngày kết thúc</label>
                            <span style="color: red">&nbsp;*</span>
                            <date-picker
                                v-model="model.ngayKetThuc"
                                format="DD/MM/YYYY"
                                id="percent"
                                placeholder="Nhập ngày kết thúc"
                                :disabled="true"
                            >
                            </date-picker>
                          </div>
                        </div>


                      </div>
                      <div class="text-end pt-2 mt-3">
                        <b-button variant="light" @click="showModalIsPublish = false" class="border-0">
                          Đóng
                        </b-button>
                        <b-button v-if="this.model.isDuyet == false" type="submit" variant="success" class="ms-1 btn-add">Duyệt
                        </b-button>
                      </div>
                    </form>
                  </b-modal>
                </div>
              </div>
            </div>
            <div class="row">
              <div class="col-12">
                <div class="row mt-4">
                  <div class="col-sm-12 col-md-6">
                    <div
                        class="col-sm-12 d-flex justify-content-left align-items-center"
                    >
                      <div
                          id="tickets-table_length"
                          class="dataTables_length m-1"
                          style="
                        display: flex;
                        justify-content: left;
                        align-items: center;
                      "
                      >
                        <div class="me-1" >Hiển thị </div>
                        <b-form-select
                            class="form-select form-select-sm"
                            v-model="perPage"
                            size="sm"
                            :options="pageOptions"
                            style="width: 70px"
                        ></b-form-select
                        >&nbsp;
                        <div style="width: 100px"> dòng </div>
                      </div>
                    </div>
                  </div>
                </div>
                <div class="table-responsive mb-0">
                  <b-table
                      class="datatables table-admin"
                      :items="myProvider"
                      :fields="fields"
                      striped
                      bordered
                      responsive="sm"
                      :per-page="perPage"
                      :current-page="currentPage"
                      :sort-by.sync="sortBy"
                      :sort-desc.sync="sortDesc"
                      :filter="filter"
                      :filter-included-fields="filterOn"
                      ref="tblList"
                      primary-key="id"
                  >
                    <template v-slot:cell(STT)="data">
                      {{ data.index + ((currentPage-1)*perPage) + 1  }}
                    </template>
                    <template v-slot:cell(isDuyet)="data">
                          <span v-if="data.item.isDuyet == false" 
                            style="
                              padding: 5px;
                              border-radius: 5px;
                              background-color: #ff00004a; 
                              color: red;"
                          >
                              Không xuất bản
                          </span>
                          <span v-else 
                            style="
                              padding: 5px;
                              border-radius: 5px;
                              background-color: #28883b; 
                              color: #fff;"
                          >
                              Xuất bản
                          </span>
                    </template>
                    <template v-slot:cell(process)="data">
                      <button
                          type="button"
                          size="sm"
                          class="btn btn-outline btn-sm"
                          v-on:click="handleUpdate(data.item.id)">
                        <i class="fas fa-pencil-alt text-success me-1"></i>
                      </button>
                      <button
                          type="button"
                          size="sm"
                          class="btn btn-outline btn-sm"
                          v-on:click="handleShowDeleteModal(data.item.id)">
                        <i class="fas fa-trash-alt text-danger me-1"></i>
                      </button>
                    </template>
                  </b-table>
                </div>
                <div class="row">
                  <b-col>
                    <div>Hiển thị {{numberOfElement}} trên tổng số {{totalRows}} dòng</div>
                  </b-col>
                  <div class="col">
                    <div
                        class="dataTables_paginate paging_simple_numbers float-end">
                      <ul class="pagination pagination-rounded mb-0">
                        <!-- pagination -->
                        <b-pagination
                            v-model="currentPage"
                            :total-rows="totalRows"
                            :per-page="perPage"
                        ></b-pagination>
                      </ul>
                    </div>
                  </div>
                </div>

              </div>
            </div>
            <b-modal
                v-model="showDeleteModal"
                centered
                title="Xóa dữ liệu"
                title-class="font-18"
                no-close-on-backdrop
            >
              <p>
                Dữ liệu xóa sẽ không được phục hồi!
              </p>
              <template #modal-footer>
                <button v-b-modal.modal-close_visit
                        class="btn btn-outline-info m-1"
                        v-on:click="showDeleteModal = false">
                  Đóng
                </button>
                <button v-b-modal.modal-close_visit
                        class="btn btn-danger btn m-1"
                        v-on:click="handleDelete">
                  Xóa
                </button>
              </template>
            </b-modal>
          </div>
        </div>
      </div>
    </div>
  </Layout>
</template>
<style>
.td-xuly {
  text-align: center;
  width: 10%
}

.td-trangthai{
  text-align: center;
}
</style>
