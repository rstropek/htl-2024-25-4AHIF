import express from "express";
import { createProxyMiddleware, fixRequestBody } from "http-proxy-middleware";
import dotenv from "dotenv";
import cors from "cors";

dotenv.config();

const app = express();

app.use(cors());

// Define the target server
const target = process.env.AZURE_ENDPOINT;

// Proxy middleware to inject header and forward requests
const proxy = createProxyMiddleware({
  target: target,
  changeOrigin: true,
  on: {
    proxyReq: (proxyReq, req, res) => {
      proxyReq.setHeader("api-key", process.env.AZURE_API_KEY);

      // Add api-version to query parameters if not already present
      if (!proxyReq.path.includes("api-version")) {
        proxyReq.path +=
          (proxyReq.path.includes("?") ? "&" : "?") +
          "api-version=2024-09-01-preview";
      }
    },
  },
});

// Apply the proxy to all requests
app.use("/", proxy);

// Start the server
app.listen(3000, () => {
  console.log("Proxy server is running on port 3000");
});
