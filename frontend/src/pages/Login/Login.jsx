import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { login } from "../../services/auth";

import {
  Box,
  Button,
  Paper,
  TextField,
  Typography,
} from "@mui/material";

export default function Login() {
  const navigate = useNavigate();

  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  const handleLogin = async () => {
    try {
      const result = await login(username, password);

      localStorage.setItem("token", result.token);
      localStorage.setItem("role", result.role);
      localStorage.setItem("username", username);

      navigate("/home");
    } catch {
      alert("Invalid username or password");
    }
  };

  return (
    <Box
      sx={{
        height: "100vh",
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        bgcolor: "#f5f5f5",
      }}
    >
      <Paper sx={{ p: 5, width: 350 }}>
        <Typography variant="h4" mb={3}>
          Media Center CMS
        </Typography>

        <TextField
          label="Username"
          fullWidth
          margin="normal"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
        />

        <TextField
          label="Password"
          type="password"
          fullWidth
          margin="normal"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />

        <Button
          variant="contained"
          fullWidth
          sx={{ mt: 3 }}
          onClick={handleLogin}
        >
          Login
        </Button>
      </Paper>
    </Box>
  );
}