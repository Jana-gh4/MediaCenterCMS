import {
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
} from "@mui/material";

export default function AuditLogsTable({ rows }) {
  return (
    <TableContainer>
      <Table>
        <TableHead>
          <TableRow>
            <TableCell>المستخدم</TableCell>
            <TableCell>الإجراء</TableCell>
            <TableCell>نوع العنصر</TableCell>
            <TableCell>المعرف</TableCell>
            <TableCell>التاريخ</TableCell>
          </TableRow>
        </TableHead>

        <TableBody>
          {rows.map((row) => (
            <TableRow key={row.auditLogId}>
              <TableCell>{row.username}</TableCell>
              <TableCell>{row.action}</TableCell>
              <TableCell>{row.entityName}</TableCell>
              <TableCell>{row.entityId}</TableCell>
              <TableCell>
                {new Date(row.timestamp).toLocaleString()}
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
}