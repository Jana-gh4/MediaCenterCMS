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

export default function VideoDialog({
  open,
  onClose,
  onSave,
  mode = "create",
  video = null,
}) {
  const [title, setTitle] = useState("");
  const [selectedVideo, setSelectedVideo] = useState(null);

  useEffect(() => {
    if (video) {
      setTitle(video.title || "");
    } else {
      setTitle("");
      setSelectedVideo(null);
    }
  }, [video, open]);

  const handleSave = () => {
    const formData = new FormData();

    formData.append("Title", title);

    if (selectedVideo) {
      formData.append("Video", selectedVideo);
    }

    onSave(formData);

    setTitle("");
    setSelectedVideo(null);
  };

  return (
    <Dialog
      open={open}
      onClose={onClose}
      maxWidth="sm"
      fullWidth
    >
      <DialogTitle>
        {mode === "create" && "إضافة فيديو"}
        {mode === "view" && "عرض الفيديو"}
      </DialogTitle>

      <DialogContent>
        <Stack spacing={3} mt={1}>
          <TextField
            label="عنوان الفيديو"
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
              اختيار فيديو

              <input
                hidden
                type="file"
                accept="video/*"
                onChange={(e) => setSelectedVideo(e.target.files[0])}
              />
            </Button>
          )}

          {mode === "view" && (
            <>
              {video?.videoPath ? (
                <Box
                  component="video"
                  controls
                  src={`${BASE_URL}${video.videoPath}`}
                  sx={{
                    width: "100%",
                    maxHeight: 350,
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
                  لا يوجد فيديو
                </Box>
              )}
            </>
          )}

          {mode !== "view" && selectedVideo && (
            <Box
              component="video"
              controls
              src={URL.createObjectURL(selectedVideo)}
              sx={{
                width: "100%",
                maxHeight: 300,
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