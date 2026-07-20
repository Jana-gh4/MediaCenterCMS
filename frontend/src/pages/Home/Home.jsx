import { useEffect, useState } from "react";

import Layout from "../../components/layout/Layout";
import PageTitle from "../../components/common/PageTitle";
import StatCard from "../../components/common/StatCard";
import ServiceCard from "../../components/common/ServiceCard";

import { getDashboard } from "../../services/dashboard";

import {
  Article,
  Image,
  VideoLibrary,
} from "@mui/icons-material";

import {
  Grid,
  Typography,
} from "@mui/material";

import { colors } from "../../colors";

export default function Home() {

  const [stats, setStats] = useState({
    newsCount: 0,
    imagesCount: 0,
    videosCount: 0,
    pendingApprovals: 0,
  });

  const loadDashboard = async () => {
    try {
      const data = await getDashboard();
      console.log(data);
      setStats(data);
    } catch (err) {
      console.error(err);
    }
  };

  useEffect(() => {
    loadDashboard();
  }, []);

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

        <Grid item xs={12} md={3}>
          <StatCard
            title="الأخبار"
            value={stats.newsCount}
          />
        </Grid>

        <Grid item xs={12} md={3}>
          <StatCard
            title="الصور"
            value={stats.imagesCount}
          />
        </Grid>

        <Grid item xs={12} md={3}>
          <StatCard
            title="الفيديوهات"
            value={stats.videosCount}
          />
        </Grid>

        <Grid item xs={12} md={3}>
          <StatCard
            title="طلبات الاعتماد"
            value={stats.pendingApprovals}
          />
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