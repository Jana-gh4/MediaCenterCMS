import { Box, Typography } from "@mui/material";
import { colors } from "../../theme";

export default function PageTitle({ title, subtitle }) {
  return (
    <Box mb={4}>
      <Typography
        variant="h4"
        sx={{
          fontWeight: "bold",
          color: colors.text,
          mb: 1,
        }}
      >
        {title}
      </Typography>

      {subtitle && (
        <Typography
          sx={{
            color: colors.subtitle,
          }}
        >
          {subtitle}
        </Typography>
      )}
    </Box>
  );
}