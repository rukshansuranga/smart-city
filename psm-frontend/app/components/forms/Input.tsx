import { Label, TextInput, HelperText } from "flowbite-react";
import { Control, UseControllerProps, useController } from "react-hook-form";

type Props = {
  label?: string;
  placeholder?: string;
  type?: string;
  showlabel?: boolean;
  className?: string;
  required?: boolean;
  control: Control;
} & UseControllerProps;

const FormField: React.FC<Props> = (props: Props) => {
  const { fieldState, field } = useController({ ...props, defaultValue: "" });

  return (
    <div className="w-full">
      {props?.showlabel && (
        <div className="mt-2 mb-1 block">
          <Label htmlFor={props.name}>{props.label}</Label>
          {props.required && <span className="text-red-600">*</span>}
        </div>
      )}
      <TextInput
        {...props}
        {...field}
        type={props.type || "text"}
        placeholder={props.placeholder}
        className={props.className}
        color={
          fieldState.error ? "failure" : !fieldState.isDirty ? "" : "success"
        }
        onChange={(e) => {
          const fieldValue =
            props.type === "number"
              ? parseFloat(e.target.value)
              : e.target.value;
          field.onChange(fieldValue);
        }}
      />
      <HelperText>{fieldState.error?.message}</HelperText>
    </div>
  );
};
export default FormField;
