import type { NextConfig } from "next";
import withFlowbiteReact from "flowbite-react/plugin/nextjs";

const nextConfig: NextConfig = {
  /* config options here */
  output: "standalone",
  distDir: ".next",
  // Enable source maps for better debugging
  productionBrowserSourceMaps: true,
  // Enable detailed error overlay in development
  webpack: (config, { dev }) => {
    if (dev) {
      config.devtool = "eval-source-map";
    }
    return config;
  },
};

export default withFlowbiteReact(nextConfig);
