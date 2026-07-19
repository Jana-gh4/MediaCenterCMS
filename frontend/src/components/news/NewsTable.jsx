import {
  IconButton,
} from "@mui/material";

import {
  Edit,
  Visibility,
} from "@mui/icons-material";

import AppTable from "../common/AppTable";

const columns = [

  {
    field: "title",
    header: "عنوان الخبر",
  },

  {
    field: "createdBy",
    header: "المنشئ",
    align: "center",
  },

  {
    field: "createdAt",
    header: "تاريخ الإنشاء",
    align: "center",

    render: (row) =>
      new Date(row.createdAt).toLocaleDateString("en-GB"),
  },

];

export default function NewsTable({
    rows,
    onView,
    onEdit,
    }) {

  return (

    <AppTable

      rows={rows}

      columns={columns}

      renderActions={(row) => (
        <>
          <IconButton onClick={() => onView(row)}>
            <Visibility />
          </IconButton>

          <IconButton onClick={() => onEdit(row)}>
            <Edit />
            </IconButton>
        </>
      )}

    />

  );

}