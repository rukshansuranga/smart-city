import { Label, HelperText, Textarea } from "flowbite-react";
import { UseControllerProps, useController } from "react-hook-form";

type Props = {
  label?: string;
  placeholder?: string;
  rows?: number;
  showlabel?: boolean;
  className?: string;
  required?: boolean;
} & UseControllerProps;

const TextAreaField: React.FC<Props> = (props: Props) => {
  const { fieldState, field } = useController({ ...props, defaultValue: "" });

  return (
    <div className="w-full">
      {props?.showlabel && (
        <div className="mt-2 mb-1 block">
          <Label htmlFor={props.name}>{props.label}</Label>
          {props.required && <span className="text-red-600">*</span>}
        </div>
      )}
      <Textarea
        {...props}
        {...field}
        placeholder={props.placeholder}
        className={props.className}
        rows={props.rows ?? 4}
        color={
          fieldState.error ? "failure" : !fieldState.isDirty ? "" : "success"
        }
      />
      <HelperText>{fieldState.error?.message}</HelperText>
    </div>
  );
};
export default TextAreaField;
