import {
  AppBar,
  Toolbar,
  Typography,
  Avatar,
  Box,
} from "@mui/material";

import { colors } from "../../theme";

export default function Header() {
  return (
    <AppBar
      position="static"
      elevation={0}
      sx={{
        bgcolor: "#fff",
        color: colors.text,
        borderRadius: 4,
        border: `1px solid ${colors.border}`,
      }}
    >
      <Toolbar>

        <Typography
          variant="h6"
          sx={{
            fontWeight: "bold",
          }}
        >
          نظام إدارة المركز الإعلامي
        </Typography>

        <Box sx={{ flexGrow: 1 }} />

        <Typography sx={{ ml: 2 }}>
          {localStorage.getItem("username")}
        </Typography>

        <Avatar
          sx={{
            bgcolor: colors.primary,
            width: 36,
            height: 36,
          }}
        >
          A
        </Avatar>

      </Toolbar>
    </AppBar>
  );
}