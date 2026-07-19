import {
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  IconButton,
  Chip,
} from "@mui/material";

import VisibilityIcon from "@mui/icons-material/Visibility";

const BASE_URL = "http://localhost:5202/";

export default function ImagesTable({ rows, onView }) {
  return (
    <TableContainer>
      <Table>
        <TableHead>
          <TableRow>
            <TableCell>الصورة</TableCell>
            <TableCell>العنوان</TableCell>
            <TableCell>الحالة</TableCell>
            <TableCell>بواسطة</TableCell>
            <TableCell>تاريخ الإنشاء</TableCell>
            <TableCell align="center">الإجراءات</TableCell>
          </TableRow>
        </TableHead>

        <TableBody>
          {rows.map((row) => (
            <TableRow key={row.galleryImageId}>
              <TableCell>
                <img
                  src={`${BASE_URL}${row.imagePath}`}
                  alt={row.title}
                  width={90}
                  height={60}
                  style={{
                    objectFit: "cover",
                    borderRadius: 8,
                    border: "1px solid #ddd",
                  }}
                />
              </TableCell>

              <TableCell>{row.title}</TableCell>

              <TableCell>
                <Chip
                  label={row.status}
                  color={
                    row.status === "Approved"
                      ? "success"
                      : row.status === "Rejected"
                      ? "error"
                      : "warning"
                  }
                  size="small"
                />
              </TableCell>

              <TableCell>{row.createdBy}</TableCell>

              <TableCell>
                {new Date(row.createdAt).toLocaleDateString()}
              </TableCell>

              <TableCell align="center">
                <IconButton onClick={() => onView(row)}>
                  <VisibilityIcon />
                </IconButton>
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
}