import moment from "moment";
const toJson = (item) => {
  return {
    id: item.id,
    name: item.name,
    noiDungTuyenTruyen: item.noiDungTuyenTruyen,
    ngayTuyenTruyen: item.ngayTuyenTruyen,
    soLanPhat: item.soLanPhat,
  };
};

const fromJson = (item) => {
  return {
    id: item.id,
    name: item.name,
    noiDungTuyenTruyen: item.noiDungTuyenTruyen,
    ngayTuyenTruyen: item.ngayTuyenTruyen,
    soLanPhat: item.soLanPhat,
  };
};

const baseJson = () => {
  return {
    id: null,
    name: null,
    noiDungTuyenTruyen: null,
    ngayTuyenTruyen: null,
    soLanPhat: 0,
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
export const lichSuModel = {
  toJson,
  fromJson,
  baseJson,
  toListModel,
};
