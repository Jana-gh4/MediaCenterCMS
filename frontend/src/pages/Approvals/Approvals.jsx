import { useEffect, useState } from "react";

import Layout from "../../components/layout/Layout";
import PageTitle from "../../components/common/PageTitle";
import ApprovalsTable from "../../components/approval/ApprovalsTable";

import { getApprovals } from "../../services/approval";

import { Paper } from "@mui/material";

export default function Approvals() {
  const [approvals, setApprovals] = useState([]);

  const loadApprovals = async () => {
    try {
      const data = await getApprovals();

      console.log(data);

      setApprovals(data);

    } catch (err) {
      console.error(err);
      alert("Failed to load approval requests.");
    }
  };

  useEffect(() => {
    loadApprovals();
  }, []);

  return (
    <Layout>
      <PageTitle
        title="طلبات الاعتماد"
        subtitle="مراجعة واعتماد المحتوى"
      />

      <Paper
        elevation={0}
        sx={{
          p: 3,
          borderRadius: 3,
          border: "1px solid #E5E7EB",
        }}
      >
        <ApprovalsTable
          approvals={approvals}
          refresh={loadApprovals}
        />
      </Paper>
    </Layout>
  );
}