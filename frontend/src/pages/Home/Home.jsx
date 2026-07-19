import Layout from "../../components/layout/Layout";
import PageTitle from "../../components/common/PageTitle";
import StatCard from "../../components/common/StatCard";
import ServiceCard from "../../components/common/ServiceCard";

import {
  Article,
  Image,
  VideoLibrary,
} from "@mui/icons-material";

import {
  Grid,
  Typography,
} from "@mui/material";

import { colors } from "../../theme";

export default function Home() {
  return (
    <Layout>

      <PageTitle
        title="لوحة التحكم"
        subtitle="مرحبًا بك في نظام إدارة المركز الإعلامي"
      />

      <Typography
        variant="h6"
        sx={{
          mb: 2,
          fontWeight: "bold",
        }}
      >
        إحصائيات سريعة
      </Typography>

      <Grid container spacing={3} mb={6}>

        <Grid item xs={12} md={4}>
          <StatCard title="الأخبار" value="12" />
        </Grid>

        <Grid item xs={12} md={4}>
          <StatCard title="الصور" value="35" />
        </Grid>

        <Grid item xs={12} md={4}>
          <StatCard title="الفيديوهات" value="8" />
        </Grid>

      </Grid>

      <Typography
        variant="h6"
        sx={{
          mb: 2,
          fontWeight: "bold",
        }}
      >
        الخدمات
      </Typography>

      <Grid container spacing={3}>

        <Grid item xs={12} md={4}>
          <ServiceCard
            icon={<Article sx={{ fontSize: 65, color: colors.primary }} />}
            title="الأخبار"
            description="إدارة الأخبار والمنشورات"
            route="/news"
          />
        </Grid>

        <Grid item xs={12} md={4}>
          <ServiceCard
            icon={<Image sx={{ fontSize: 65, color: colors.primary }} />}
            title="الصور"
            description="إدارة معرض الصور"
            route="/images"
          />
        </Grid>

        <Grid item xs={12} md={4}>
          <ServiceCard
            icon={<VideoLibrary sx={{ fontSize: 65, color: colors.primary }} />}
            title="الفيديوهات"
            description="إدارة معرض الفيديوهات"
            route="/videos"
          />
        </Grid>

      </Grid>

    </Layout>
  );
}