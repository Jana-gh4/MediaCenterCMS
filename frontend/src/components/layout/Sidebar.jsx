import {
  Box,
  List,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Typography,
} from "@mui/material";

import {
  Home,
  Article,
  Image,
  VideoLibrary,
  TaskAlt,
  History,
  Logout,
} from "@mui/icons-material";

import { Link } from "react-router-dom";
import { colors } from "../../theme";

const menuItems = [
  { text: "الرئيسية", icon: <Home />, path: "/home" },
  { text: "الأخبار", icon: <Article />, path: "/news" },
  { text: "الصور", icon: <Image />, path: "/images" },
  { text: "الفيديوهات", icon: <VideoLibrary />, path: "/videos" },
  { text: "طلبات الاعتماد", icon: <TaskAlt />, path: "/approvals" },
  { text: "سجل العمليات", icon: <History />, path: "/auditlogs" },
];

export default function Sidebar() {
  return (
    <Box
        sx={{
            width: 260,
            bgcolor: "#fff",
            borderRadius: 4,
            boxShadow: 1,
            minHeight: "calc(100vh - 32px)",
            border: `1px solid ${colors.border}`,
        }}
    >
      <Typography
        variant="h6"
        sx={{
          textAlign: "center",
          py: 3,
          fontWeight: "bold",
          color: colors.primary,
        }}
      >
        المركز الإعلامي
      </Typography>

      <List>
        {menuItems.map((item) => (
          <ListItemButton
            key={item.text}
            component={Link}
            to={item.path}
            sx={{
              py: 1.5,
              mx: 1,
              borderRadius: 2,
              mb: 1,
            }}
          >
            <ListItemIcon
              sx={{
                color: colors.primary,
                minWidth: 40,
              }}
            >
              {item.icon}
            </ListItemIcon>

            <ListItemText
              primary={item.text}
              sx={{ textAlign: "right" }}
            />
          </ListItemButton>
        ))}

        <ListItemButton
          sx={{
            mt: 3,
            mx: 1,
            borderRadius: 2,
          }}
        >
          <ListItemIcon
            sx={{
              color: "red",
              minWidth: 40,
            }}
          >
            <Logout />
          </ListItemIcon>

          <ListItemText
            primary="تسجيل الخروج"
            sx={{ textAlign: "right" }}
          />
        </ListItemButton>
      </List>
    </Box>
  );
}