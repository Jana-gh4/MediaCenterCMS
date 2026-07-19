import {
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
} from "@mui/material";

export default function AppTable({
  columns,
  rows,
  renderActions,
}) {
  return (
    <TableContainer
      component={Paper}
      sx={{
        borderRadius: 3,
        border: "1px solid #E5E7EB",
        boxShadow: "none",
      }}
    >
      <Table dir="rtl">

        <TableHead>
          <TableRow
            sx={{
              bgcolor: "#F8FAFC",
            }}
          >
            {columns.map((column) => (
              <TableCell
                key={column.field}
                align={column.align || "right"}
                sx={{
                  fontWeight: 700,
                }}
              >
                {column.header}
              </TableCell>
            ))}

            {renderActions && (
              <TableCell
                align="center"
                sx={{
                  fontWeight: 700,
                }}
              >
                الإجراءات
              </TableCell>
            )}
          </TableRow>
        </TableHead>

        <TableBody>

          {rows.map((row) => (
            <TableRow
              key={row.newsId}
              hover
            >
              {columns.map((column) => (
                <TableCell
                  key={column.field}
                  align={column.align || "right"}
                >
                  {column.render
                    ? column.render(row)
                    : row[column.field]}
                </TableCell>
              ))}

              {renderActions && (
                <TableCell align="center">
                  {renderActions(row)}
                </TableCell>
              )}

            </TableRow>
          ))}

        </TableBody>

      </Table>
    </TableContainer>
  );
}