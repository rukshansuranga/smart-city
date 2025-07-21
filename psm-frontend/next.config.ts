import type { NextConfig } from "next";
import withFlowbiteReact from "flowbite-react/plugin/nextjs";

const nextConfig: NextConfig = {
  /* config options here */
  output: "standalone",
  distDir: ".next",
};

export default withFlowbiteReact(nextConfig);
