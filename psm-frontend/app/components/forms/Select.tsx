import { Label, HelperText, Select } from "flowbite-react";
import { JSX } from "react";
import { UseControllerProps, useController } from "react-hook-form";
import { Option } from "@/types"; // Assuming Option type is defined in types/index.ts

type Props = {
  label?: string;
  placeholder?: string;
  value?: string;
  showlabel?: boolean;
  className?: string;
  required?: boolean;
  options: Option[];
} & UseControllerProps;

const SelectField = (props: Props): JSX.Element => {
  const { fieldState, field } = useController({ ...props, defaultValue: "" });

  return (
    <div className="w-full">
      {props?.showlabel && (
        <div className="mt-2 mb-1 block">
          <Label htmlFor={props.name}>{props.label}</Label>
          {props.required && <span className="text-red-600">*</span>}
        </div>
      )}
      <Select {...props} {...field}>
        {props?.options.map((option) => (
          <option key={option.value} value={option.value}>
            {option.text}
          </option>
        ))}
      </Select>
      <HelperText>{fieldState.error?.message}</HelperText>
    </div>
  );
};
export default SelectField;
