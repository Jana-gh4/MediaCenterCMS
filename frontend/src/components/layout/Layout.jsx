import { Box } from "@mui/material";
import Sidebar from "./Sidebar";
import Header from "./Header";
import { colors } from "../../colors";

export default function Layout({ children }) {
  return (
    <Box
      dir="rtl"
      sx={{
        background: colors.background,
        minHeight: "100vh",
        p: 2,
      }}
    >
      <Box
        sx={{
          display: "flex",
          flexDirection: "row-reverse",
          gap: 2,
        }}
      >
        {/* Main Content */}
        <Box sx={{ flex: 1 }}>
          <Header />

          <Box
            sx={{
              mt: 2,
              bgcolor: "#fff",
              borderRadius: 4,
              p: 5,
              minHeight: "calc(100vh - 110px)",
              border: "1px solid #E5E7EB",
              boxShadow: "none",
            }}
          >
            {children}
          </Box>
        </Box>

        {/* Sidebar */}
        <Sidebar />
      </Box>
    </Box>
  );
}