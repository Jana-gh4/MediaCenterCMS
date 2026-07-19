import { useEffect, useState } from "react";

import Layout from "../../components/layout/Layout";
import PageTitle from "../../components/common/PageTitle";
import VideosTable from "../../components/videos/VideosTable";
import VideoDialog from "../../components/videos/VideoDialog";

import {
  getVideos,
  createVideo,
} from "../../services/galleryVideo";

import { Paper, Box, Button } from "@mui/material";
import AddIcon from "@mui/icons-material/Add";

export default function Videos() {
  const [videos, setVideos] = useState([]);
  const [openDialog, setOpenDialog] = useState(false);

  const [selectedVideo, setSelectedVideo] = useState(null);
  const [dialogMode, setDialogMode] = useState("create");

  const loadVideos = async () => {
    try {
      const data = await getVideos();

      console.log(data);

      setVideos(data);
    } catch (err) {
      console.error(err);
    }
  };

  const handleSave = async (formData) => {
    try {
      console.log("Saving...");

      await createVideo(formData);

      console.log("Saved!");

      setOpenDialog(false);

      await loadVideos();
    } catch (err) {
      console.error(err);
      alert("Failed to save video");
    }
  };

  const handleView = (video) => {
    setSelectedVideo(video);
    setDialogMode("view");
    setOpenDialog(true);
  };

  useEffect(() => {
    loadVideos();
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
          title="إدارة الفيديوهات"
          subtitle="إدارة معرض الفيديوهات"
        />

        <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={() => {
            setSelectedVideo(null);
            setDialogMode("create");
            setOpenDialog(true);
          }}
        >
          إضافة فيديو
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
        <VideosTable
          rows={videos}
          onView={handleView}
        />

        <VideoDialog
          open={openDialog}
          onClose={() => setOpenDialog(false)}
          onSave={handleSave}
          mode={dialogMode}
          video={selectedVideo}
        />
      </Paper>
    </Layout>
  );
}