import { useEffect, useState } from "react";

import Layout from "../../components/layout/Layout";
import PageTitle from "../../components/common/PageTitle";
import AuditLogsTable from "../../components/audit/AuditLogsTable";

import { getAuditLogs } from "../../services/audit";

import { Paper } from "@mui/material";

export default function AuditLogs() {
  const [logs, setLogs] = useState([]);

  const loadLogs = async () => {
    try {
      const data = await getAuditLogs();
      setLogs(data);
    } catch (err) {
      console.error(err);
    }
  };

  useEffect(() => {
    loadLogs();
  }, []);

  return (
    <Layout>
      <PageTitle
        title="سجل العمليات"
        subtitle="عرض جميع العمليات المنفذة"
      />

      <Paper
        elevation={0}
        sx={{
          p: 3,
          borderRadius: 3,
          border: "1px solid #E5E7EB",
        }}
      >
        <AuditLogsTable rows={logs} />
      </Paper>
    </Layout>
  );
}