import moment from "moment";
const toJson = (item) => {
  return {
    id: item.id,
    name: item.name,
    tenDonVi: item.tenDonVi,
    moTa: item.moTa,
    ngayBatDauTuyenTruyen: item.ngayBatDauTuyenTruyen,
    ngayKetThuc: item.ngayKetThuc,
    isDuyet: item.isDuyet,
    lichSu: item.lichSu,
  };
};

const fromJson = (item) => {
  return {
    id: item.id,
    name: item.name,
    tenDonVi: item.tenDonVi,
    moTa: item.moTa,
    ngayBatDauTuyenTruyen: item.ngayBatDauTuyenTruyen,
    ngayKetThuc: item.ngayKetThuc,
    isDuyet: item.isDuyet,
    lichSu: item.lichSu,
  };
};

const baseJson = () => {
  return {
    id: null,
    name: null,
    tenDonVi: null,
    moTa: null,
    ngayBatDauTuyenTruyen: null,
    ngayKetThuc: null,
    isDuyet: false,
    lichSu: null,
  };
};

const toListModel = (items) => {
  if (items.length > 0) {
    let data = [];
    items.map((value, index) => {
      data.push(fromJson(value));
    });
    return data ?? [];
  }
  return [];
};
export const baiThiModel = {
  toJson,
  fromJson,
  baseJson,
  toListModel,
};
