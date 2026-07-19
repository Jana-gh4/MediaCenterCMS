import { Card, CardContent, Typography } from "@mui/material";
import { colors } from "../../theme";

export default function StatCard({ title, value }) {
  return (
    <Card
      sx={{
        flex: 1,
        borderRadius: 4,
        boxShadow: 1,
        textAlign: "center",
      }}
    >
      <CardContent>

        <Typography
          variant="h4"
          sx={{
            color: colors.primary,
            fontWeight: "bold",
          }}
        >
          {value}
        </Typography>

        <Typography
          sx={{
            mt: 1,
            color: colors.subtitle,
          }}
        >
          {title}
        </Typography>

      </CardContent>
    </Card>
  );
}