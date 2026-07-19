import { useEffect, useState } from "react";

import Layout from "../../components/layout/Layout";
import PageTitle from "../../components/common/PageTitle";
import NewsTable from "../../components/news/NewsTable";

import { getNews } from "../../services/news";

import { Paper, Box, Button } from "@mui/material";
import AddIcon from "@mui/icons-material/Add";

import NewsDialog from "../../components/news/NewsDialog";

import { createNews, updateNews } from "../../services/news";

export default function News() {
  const [news, setNews] = useState([]);
  const [openDialog, setOpenDialog] = useState(false);

  const [selectedNews, setSelectedNews] = useState(null);
  const [dialogMode, setDialogMode] = useState("create");

  const loadNews = async () => {
    try {
      const data = await getNews();

      console.log(data);

      setNews(data);

    } catch (err) {
      console.error(err);
    }
  };

  const handleSave = async (formData) => {
    try {
      console.log("Saving...");

      if (dialogMode === "create") {
        await createNews(formData);
      } else if (dialogMode === "edit") {
        await updateNews(selectedNews.newsId, formData);
      }

      console.log("Saved!");

      setOpenDialog(false);

      await loadNews();

    } catch (err) {
      console.error(err);
      alert("Failed to save news");
    }
  };

  const handleView = (newsItem) => {
    console.log(newsItem);

    setSelectedNews(newsItem);
    setDialogMode("view");
    setOpenDialog(true);
  };

  const handleEdit = (newsItem) => {
    setSelectedNews(newsItem);
    setDialogMode("edit");
    setOpenDialog(true);
  };

    useEffect(() => {
      loadNews();
    }, []);

  return (
    <Layout>
      <Box
        display="flex"
        justifyContent="space-between"
        alignItems="center"
        mb={3}
      >
        <PageTitle
          title="إدارة الأخبار"
          subtitle="إدارة الأخبار والمنشورات"
        />

        <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={() => {
            setSelectedNews(null);
            setDialogMode("create");
            setOpenDialog(true);
          }}
        >
          إضافة خبر
        </Button>
      </Box>

      <Paper
        elevation={0}
        sx={{
          p: 3,
          borderRadius: 3,
          border: "1px solid #E5E7EB",
        }}
      >
        <NewsTable
          rows={news}
          onView={handleView}
          onEdit={handleEdit}
        />

        <NewsDialog
          open={openDialog}
          onClose={() => setOpenDialog(false)}
          onSave={handleSave}
          mode={dialogMode}
          news={selectedNews}
        />
      </Paper>
    </Layout>
  );
}