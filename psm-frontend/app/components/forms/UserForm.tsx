"use client";
import { useForm } from "react-hook-form";
import * as z from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { Button, Label, Spinner } from "flowbite-react";
import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import toast from "react-hot-toast";

import Input from "./Input";
import SelectField from "./Select";
import { CreateUserRequest, User, Council } from "@/types";
import { AuthType } from "@/enums";

// Type for update requests (without username)
type UpdateUserRequest = Omit<CreateUserRequest, "username">;

// Form data type that matches our schema
type FormData = z.infer<typeof userSchema>;

import {
  createUser,
  updateUser,
  getAllCouncils,
} from "@/app/api/client/userActions";

// Validation schema for both create and update (username optional for updates)
const userSchema = z.object({
  username: z
    .string()
    .trim()
    .min(3, "Username must be at least 3 characters")
    .max(50, "Username cannot exceed 50 characters")
    .regex(
      /^[a-zA-Z0-9_-]+$/,
      "Username can only contain letters, numbers, underscores, and hyphens"
    )
    .optional()
    .or(z.literal("")), // Allow empty string for update mode
  mobile: z.string().trim().min(1, "Mobile is required"),
  firstName: z
    .string()
    .trim()
    .min(1, "First name is required")
    .max(255, "First name cannot exceed 255 characters"),
  lastName: z
    .string()
    .trim()
    .min(1, "Last name is required")
    .max(255, "Last name cannot exceed 255 characters"),
  email: z
    .string()
    .trim()
    .min(1, "Email is required")
    .email("Invalid email format")
    .max(255, "Email cannot exceed 255 characters"),
  addressLine1: z.string().optional(),
  addressLine2: z.string().optional(),
  city: z.string().trim().min(1, "City is required"),
  designation: z.string().optional(),
  authType: z.coerce.number().int().optional(),
  council: z.string().trim().min(1, "Council is required"),
  password: z.string().optional(),
});

type UserFormProps = {
  userId?: string;
  initialData?: User | null;
  onSuccess?: () => void;
};

export default function UserForm({
  userId,
  initialData,
  onSuccess,
}: UserFormProps) {
  const router = useRouter();
  const [isLoading, setIsLoading] = useState(false);
  const [councils, setCouncils] = useState<Council[]>([]);
  const [loadingCouncils, setLoadingCouncils] = useState(true);

  // Extend schema with custom validation for username in create mode
  const schema = userSchema.superRefine((data, ctx) => {
    if (!userId && (!data.username || data.username.trim() === "")) {
      ctx.addIssue({
        code: "custom",
        message: "Username is required",
        path: ["username"],
      });
    }
  });

  const {
    control,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<FormData>({
    resolver: zodResolver(schema),
    mode: "onChange",
    defaultValues: {
      username: "",
      mobile: "",
      firstName: "",
      lastName: "",
      email: "",
      addressLine1: "",
      addressLine2: "",
      city: "",
      designation: "",
      authType: undefined,
      council: "",
      password: "",
    },
  });

  // Load councils on component mount
  useEffect(() => {
    const loadCouncils = async () => {
      try {
        setLoadingCouncils(true);
        const response = await getAllCouncils();
        if (response.isSuccess) {
          setCouncils(response.data);
        } else {
          toast.error("Failed to load councils");
        }
      } catch (error) {
        console.error("Error loading councils:", error);
        toast.error("Failed to load councils");
      } finally {
        setLoadingCouncils(false);
      }
    };

    loadCouncils();
  }, []);

  // Populate form with initial data for edit mode
  useEffect(() => {
    if (initialData) {
      reset({
        username: initialData.username || "",
        mobile: initialData.mobile || "",
        firstName: initialData.firstName || "",
        lastName: initialData.lastName || "",
        email: initialData.email || "",
        addressLine1: initialData.addressLine1 || "",
        addressLine2: initialData.addressLine2 || "",
        city: initialData.city || "",
        designation: initialData.designation || "",
        authType: initialData.authType || undefined,
        council: initialData.council || "",
        password: "", // Don't populate password for security
      });
    }
  }, [initialData, reset]);

  const onSubmit = async (data: FormData) => {
    try {
      setIsLoading(true);

      let response;
      if (userId && initialData) {
        // Update existing user - create update data without username
        const updateData: UpdateUserRequest = {
          mobile: data.mobile,
          firstName: data.firstName,
          lastName: data.lastName,
          email: data.email,
          addressLine1: data.addressLine1,
          addressLine2: data.addressLine2,
          city: data.city,
          designation: data.designation,
          authType: data.authType,
          council: data.council,
          password: data.password,
        };
        response = await updateUser(userId, updateData);
        if (response.isSuccess) {
          toast.success("User updated successfully!");
        } else {
          toast.error(response.message || "Failed to update user");
          return;
        }
      } else {
        // Create new user
        if (!data.username) {
          toast.error("Username is required for creating a user");
          return;
        }

        const createData: CreateUserRequest = {
          username: data.username,
          mobile: data.mobile,
          firstName: data.firstName,
          lastName: data.lastName,
          email: data.email,
          addressLine1: data.addressLine1,
          addressLine2: data.addressLine2,
          city: data.city,
          designation: data.designation,
          authType: data.authType,
          council: data.council,
          password: data.password,
        };

        response = await createUser(createData);
        if (response.isSuccess) {
          toast.success("User created successfully!");
        } else {
          toast.error(response.message || "Failed to create user");
          return;
        }
      }

      if (onSuccess) {
        onSuccess();
      } else {
        router.push("/user");
      }
    } catch (error) {
      console.error("Error saving user:", error);
      toast.error("An unexpected error occurred");
    } finally {
      setIsLoading(false);
    }
  };

  // Prepare council options for select field
  const councilOptions = [
    { value: "", text: "Select council" },
    ...councils.map((council) => ({
      value: council.name,
      text: council.name,
    })),
  ];

  return (
    <div className="max-w-4xl mx-auto p-6 bg-white rounded-lg shadow-md">
      <div className="mb-6">
        <h2 className="text-2xl font-bold text-gray-900">
          {userId ? "Update User" : "Create New User"}
        </h2>
        <p className="text-gray-600 mt-1">
          {userId ? "Update user information" : "Add a new user to the system"}
        </p>
      </div>

      <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
        {/* Personal Information Section */}
        <div className="bg-gray-50 p-4 rounded-lg">
          <h3 className="text-lg font-semibold text-gray-800 mb-4">
            Personal Information
          </h3>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            {!userId && (
              <div>
                <Label htmlFor="username" className="mb-2 block">
                  Username <span className="text-red-500">*</span>
                </Label>
                <Input
                  name="username"
                  control={control}
                  placeholder="Enter username"
                  className=""
                />
                {errors.username && (
                  <div className="text-red-500 text-sm mt-1">
                    {errors.username.message}
                  </div>
                )}
              </div>
            )}

            <div>
              <Label htmlFor="email" className="mb-2 block">
                Email <span className="text-red-500">*</span>
              </Label>
              <Input
                name="email"
                type="email"
                control={control}
                placeholder="Enter email address"
              />
              {errors.email && (
                <div className="text-red-500 text-sm mt-1">
                  {errors.email.message}
                </div>
              )}
            </div>

            <div>
              <Label htmlFor="firstName" className="mb-2 block">
                First Name <span className="text-red-500">*</span>
              </Label>
              <Input
                name="firstName"
                control={control}
                placeholder="Enter first name"
              />
              {errors.firstName && (
                <div className="text-red-500 text-sm mt-1">
                  {errors.firstName.message}
                </div>
              )}
            </div>

            <div>
              <Label htmlFor="lastName" className="mb-2 block">
                Last Name <span className="text-red-500">*</span>
              </Label>
              <Input
                name="lastName"
                control={control}
                placeholder="Enter last name"
              />
              {errors.lastName && (
                <div className="text-red-500 text-sm mt-1">
                  {errors.lastName.message}
                </div>
              )}
            </div>

            <div>
              <Label htmlFor="mobile" className="mb-2 block">
                Mobile <span className="text-red-500">*</span>
              </Label>
              <Input
                name="mobile"
                control={control}
                placeholder="Enter mobile number"
              />
              {errors.mobile && (
                <div className="text-red-500 text-sm mt-1">
                  {errors.mobile.message}
                </div>
              )}
            </div>
          </div>
        </div>

        {/* Address Information Section */}
        <div className="bg-gray-50 p-4 rounded-lg">
          <h3 className="text-lg font-semibold text-gray-800 mb-4">
            Address Information
          </h3>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <Label htmlFor="addressLine1" className="mb-2 block">
                Address Line 1
              </Label>
              <Input
                name="addressLine1"
                control={control}
                placeholder="Enter address line 1"
              />
              {errors.addressLine1 && (
                <div className="text-red-500 text-sm mt-1">
                  {errors.addressLine1.message}
                </div>
              )}
            </div>

            <div>
              <Label htmlFor="addressLine2" className="mb-2 block">
                Address Line 2
              </Label>
              <Input
                name="addressLine2"
                control={control}
                placeholder="Enter address line 2"
              />
              {errors.addressLine2 && (
                <div className="text-red-500 text-sm mt-1">
                  {errors.addressLine2.message}
                </div>
              )}
            </div>

            <div>
              <Label htmlFor="city" className="mb-2 block">
                City <span className="text-red-500">*</span>
              </Label>
              <Input name="city" control={control} placeholder="Enter city" />
              {errors.city && (
                <div className="text-red-500 text-sm mt-1">
                  {errors.city.message}
                </div>
              )}
            </div>
          </div>
        </div>

        {/* Professional Information Section */}
        <div className="bg-gray-50 p-4 rounded-lg">
          <h3 className="text-lg font-semibold text-gray-800 mb-4">
            Professional Information
          </h3>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <Label htmlFor="designation" className="mb-2 block">
                Designation
              </Label>
              <Input
                name="designation"
                control={control}
                placeholder="Enter designation"
              />
              {errors.designation && (
                <div className="text-red-500 text-sm mt-1">
                  {errors.designation.message}
                </div>
              )}
            </div>

            <div>
              <Label htmlFor="council" className="mb-2 block">
                Council <span className="text-red-500">*</span>
              </Label>
              {loadingCouncils ? (
                <div className="flex items-center justify-center p-2">
                  <Spinner size="sm" />
                  <span className="ml-2">Loading councils...</span>
                </div>
              ) : (
                <>
                  <SelectField
                    name="council"
                    control={control}
                    options={councilOptions}
                  />
                  {errors.council && (
                    <div className="text-red-500 text-sm mt-1">
                      {errors.council.message}
                    </div>
                  )}
                </>
              )}
            </div>

            <div>
              <Label htmlFor="authType" className="mb-2 block">
                Authentication Type
              </Label>
              <SelectField
                name="authType"
                control={control}
                options={[
                  { value: "", text: "Select Auth Type" },
                  ...Object.entries(AuthType)
                    .filter(([, value]) => typeof value === "number")
                    .map(([key, value]) => ({
                      value,
                      text: key.replace(/([A-Z])/g, " $1").trim(),
                    })),
                ]}
              />
              {errors.authType && (
                <div className="text-red-500 text-sm mt-1">
                  {errors.authType.message}
                </div>
              )}
            </div>

            {!userId && (
              <div>
                <Label htmlFor="password" className="mb-2 block">
                  Password
                </Label>
                <Input
                  name="password"
                  type="password"
                  control={control}
                  placeholder="Enter password (optional)"
                />
                {errors.password && (
                  <div className="text-red-500 text-sm mt-1">
                    {errors.password.message}
                  </div>
                )}
              </div>
            )}
          </div>
        </div>

        {/* Action Buttons */}
        <div className="flex gap-4 pt-4">
          <Button
            type="submit"
            disabled={isLoading}
            className="flex-1 bg-blue-600 hover:bg-blue-700"
          >
            {isLoading ? (
              <>
                <Spinner size="sm" className="mr-2" />
                {userId ? "Updating..." : "Creating..."}
              </>
            ) : (
              <>{userId ? "Update User" : "Create User"}</>
            )}
          </Button>

          <Button
            type="button"
            color="gray"
            onClick={() => router.push("/user")}
            className="flex-1"
          >
            Cancel
          </Button>
        </div>
      </form>
    </div>
  );
}
