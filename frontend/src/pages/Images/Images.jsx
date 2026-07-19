import { useEffect, useState } from "react";

import Layout from "../../components/layout/Layout";
import PageTitle from "../../components/common/PageTitle";
import ImagesTable from "../../components/images/ImagesTable";
import ImageDialog from "../../components/images/ImageDialog";

import {
  getImages,
  createImage,
} from "../../services/galleryImage";

import { Paper, Box, Button } from "@mui/material";
import AddIcon from "@mui/icons-material/Add";

export default function Images() {
  const [images, setImages] = useState([]);
  const [openDialog, setOpenDialog] = useState(false);

  const [selectedImage, setSelectedImage] = useState(null);
  const [dialogMode, setDialogMode] = useState("create");

  const loadImages = async () => {
    try {
      const data = await getImages();

      console.log(data);

      setImages(data);
    } catch (err) {
      console.error(err);
    }
  };

  const handleSave = async (formData) => {
    try {
      console.log("Saving...");

      await createImage(formData);

      console.log("Saved!");

      setOpenDialog(false);

      await loadImages();
    } catch (err) {
      console.error(err);
      alert("Failed to save image");
    }
  };

  const handleView = (image) => {
    setSelectedImage(image);
    setDialogMode("view");
    setOpenDialog(true);
  };

  useEffect(() => {
    loadImages();
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
          title="إدارة الصور"
          subtitle="إدارة معرض الصور"
        />

        <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={() => {
            setSelectedImage(null);
            setDialogMode("create");
            setOpenDialog(true);
          }}
        >
          إضافة صورة
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
        <ImagesTable
          rows={images}
          onView={handleView}
        />

        <ImageDialog
          open={openDialog}
          onClose={() => setOpenDialog(false)}
          onSave={handleSave}
          mode={dialogMode}
          image={selectedImage}
        />
      </Paper>
    </Layout>
  );
}