import { Card, CardContent, Typography } from "@mui/material";
import { colors } from "../../theme";
import { useNavigate } from "react-router-dom";

export default function ServiceCard({
  icon,
  title,
  description,
  route,
}) {
  const navigate = useNavigate();

  return (
    <Card
      onClick={() => navigate(route)}
      sx={{
        width: 300,
        borderRadius: 4,
        cursor: "pointer",
        transition: "0.3s",
        "&:hover": {
          transform: "translateY(-6px)",
          boxShadow: 6,
        },
      }}
    >
      <CardContent
        sx={{
          textAlign: "center",
          py: 5,
        }}
      >
        {icon}

        <Typography
          variant="h5"
          mt={2}
          fontWeight="bold"
        >
          {title}
        </Typography>

        <Typography
          sx={{
            mt: 2,
            color: colors.subtitle,
          }}
        >
          {description}
        </Typography>
      </CardContent>
    </Card>
  );
}