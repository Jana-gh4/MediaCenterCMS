import { useState, useEffect } from "react";

import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  Stack,
  TextField,
  Box,
} from "@mui/material";

const BASE_URL = "http://localhost:5202/";

export default function ImageDialog({
  open,
  onClose,
  onSave,
  mode = "create",
  image = null,
}) {
  const [title, setTitle] = useState("");
  const [selectedImage, setSelectedImage] = useState(null);

  useEffect(() => {
    if (image) {
      setTitle(image.title || "");
    } else {
      setTitle("");
      setSelectedImage(null);
    }
  }, [image, open]);

  const handleSave = () => {
    const formData = new FormData();

    formData.append("Title", title);

    if (selectedImage) {
      formData.append("Image", selectedImage);
    }

    onSave(formData);

    setTitle("");
    setSelectedImage(null);
  };

  return (
    <Dialog
      open={open}
      onClose={onClose}
      maxWidth="sm"
      fullWidth
    >
      <DialogTitle>
        {mode === "create" && "إضافة صورة"}
        {mode === "view" && "عرض الصورة"}
      </DialogTitle>

      <DialogContent>
        <Stack spacing={3} mt={1}>
          <TextField
            label="عنوان الصورة"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            fullWidth
            disabled={mode === "view"}
          />

          {mode !== "view" && (
            <Button
              variant="outlined"
              component="label"
            >
              اختيار صورة

              <input
                hidden
                type="file"
                accept="image/*"
                onChange={(e) =>
                  setSelectedImage(e.target.files[0])
                }
              />
            </Button>
          )}

          {mode === "view" && (
            <>
              {image?.imagePath ? (
                <Box
                  component="img"
                  src={`${BASE_URL}${image.imagePath}`}
                  alt={image.title}
                  sx={{
                    width: "100%",
                    maxHeight: 350,
                    objectFit: "contain",
                    borderRadius: 2,
                    border: "1px solid #ddd",
                  }}
                />
              ) : (
                <Box
                  sx={{
                    p: 2,
                    textAlign: "center",
                    border: "1px dashed #ccc",
                    borderRadius: 2,
                    color: "text.secondary",
                  }}
                >
                  لا توجد صورة
                </Box>
              )}
            </>
          )}

          {mode !== "view" && selectedImage && (
            <Box
              component="img"
              src={URL.createObjectURL(selectedImage)}
              alt="Preview"
              sx={{
                width: "100%",
                maxHeight: 300,
                objectFit: "contain",
                borderRadius: 2,
                border: "1px solid #ddd",
              }}
            />
          )}
        </Stack>
      </DialogContent>

      <DialogActions>
        <Button onClick={onClose}>
          {mode === "view" ? "إغلاق" : "إلغاء"}
        </Button>

        {mode !== "view" && (
          <Button
            variant="contained"
            onClick={handleSave}
          >
            حفظ
          </Button>
        )}
      </DialogActions>
    </Dialog>
  );
}