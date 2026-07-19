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

export default function VideosTable({ rows, onView }) {
  return (
    <TableContainer>
      <Table>
        <TableHead>
          <TableRow>
            <TableCell>الفيديو</TableCell>
            <TableCell>العنوان</TableCell>
            <TableCell>الحالة</TableCell>
            <TableCell>بواسطة</TableCell>
            <TableCell>تاريخ الإنشاء</TableCell>
            <TableCell align="center">الإجراءات</TableCell>
          </TableRow>
        </TableHead>

        <TableBody>
          {rows.map((row) => (
            <TableRow key={row.galleryVideoId}>
            <TableCell>
                {row.videoPath ? (
                <video
                    src={`${BASE_URL}${row.videoPath}`}
                    controls
                    style={{
                    width: 180,
                    height: 100,
                    objectFit: "cover",
                    borderRadius: 8,
                    border: "1px solid #ddd",
                    }}
                />
                ) : (
                "لا يوجد فيديو"
                )}
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